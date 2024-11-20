using AutoMapper;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Configuration;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Project;
using GenAIChat.Presentation.API.Controllers.Project.Request;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController(ProjectApplication application, PromptConfiguration promptConfiguration, IMapper mapper)
        : ControllerBase
    {
        private static async Task<DocumentDomain[]> ConvertFileFormToDocumentDomain(IEnumerable<IFormFile> files)
        {
            IEnumerable<Task<DocumentDomain>> convertions = files.Select(async file =>
            {
                var buffer = new byte[file.Length];
                await file.OpenReadStream().ReadAsync(buffer);
                return new DocumentDomain(file.FileName, file.ContentType, file.Length, buffer);
            });
            var documents = await Task.WhenAll(convertions);
            return documents;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllAsync(options);
            return Ok(mapper.Map<Paged<ProjectItemDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await application.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(mapper.Map<ProjectDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProjectCreateRequest request)
        {
            // check
            if (!ModelState.IsValid) return BadRequest(new ErrorDto(ModelState));

            // action
            try
            {
                // create project
                var result = await application.CreateAsync(
                    request.Name,
                    promptConfiguration.UserStories,
                    await ConvertFileFormToDocumentDomain(request.Files));

                return Ok(mapper.Map<ProjectDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProjectUpdateRequest request)
        {
            // check
            if (!ModelState.IsValid) return BadRequest(new ErrorDto(ModelState));

            // action
            try
            {
                var result = await application.UpdateAsync(new ProjectDomain(id, request.Name, request.Prompt));

                if (result is null) return NotFound();

                return Ok(mapper.Map<ProjectDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await application.DeleteAsync(id);
            return Ok(mapper.Map<ProjectDto>(result));
        }
    }
}
