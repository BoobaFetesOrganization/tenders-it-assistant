using AutoMapper;
using GenAIChat.Application;
using GenAIChat.Domain;
using GenAIChat.Domain.Common;
using GenAIChat.Infrastructure.Configuration;
using GenAIChat.Presentation.API.Controllers.Request;
using GenAIChat.Presentation.API.Controllers.Response;
using GenAIChat.Presentation.API.Controllers.Response.Project;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GenAIChat.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController(ProjectApplication application, PromptConfiguration promptConfiguration, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? offset, [FromQuery] int? limit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllAsync(options);
            return Ok(mapper.Map<Paged<ProjectDto>>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProjectCreateRequest request)
        {
            // check
            if (!ModelState.IsValid) return BadRequest(new ErrorResponse(ModelState));

            // action
            try
            {
                // convert uploaded files to DocumentDomain
                var convertions = request.Files.Select(async file =>
                {
                    var buffer = new byte[file.Length];
                    await file.OpenReadStream().ReadAsync(buffer);
                    return new DocumentDomain(file.FileName, file.ContentType, file.Length, buffer);
                });
                var documents = await Task.WhenAll(convertions);

                // create project
                ProjectDomain result = await application.CreateAsync(request.Name, promptConfiguration.UserStories, documents);

                // store physically the prompt and the documents - its not a part of the application layer, the presentation need this behavior
                // may be this will moved to the application layer in the future if you consider this as a business logic
                // but is is already in the database....
                if (result is not null && result.Id > 0)
                {
                    var rootDir = Path.Combine(Path.GetDirectoryName(webHostEnvironment.ContentRootPath)!, "projects");
                    string directory = Path.Combine(rootDir, result.Id.ToString());
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                    var fileInfos = new[]
                    {
                        new { FileName = "prompt.request.txt", Value = Encoding.Unicode.GetBytes(result.Prompt) },
                        new { FileName = "prompt.response.txt", Value =  Encoding.Unicode.GetBytes(result.PromptResponse.Text) }
                    }.ToList();
                    fileInfos.AddRange(result.Documents.Select(document => new { FileName = document.Name, Value = document.Content }));

                    var fileCreationActions = fileInfos.Select(async item =>
                    {
                        var promptPath = Path.Combine(directory, item.FileName);
                        using var fs = new FileStream(promptPath, FileMode.Create, FileAccess.ReadWrite);
                        await fs.WriteAsync(item.Value);
                    });
                    await Task.WhenAll(fileCreationActions);
                }

                return Ok(mapper.Map<ProjectCreateDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}
