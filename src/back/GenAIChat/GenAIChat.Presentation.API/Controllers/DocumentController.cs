using AutoMapper;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Dto;
using GenAIChat.Presentation.API.Controllers.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/[controller]")]
    public class DocumentController(DocumentApplication application, IMapper mapper)
        : ControllerBase
    {

        private static async Task<byte[]?> ReadFile(IFormFile file)
        {
            if (file is null || file.Length == 0) return null;

            // read file content
            byte[] buffer = new byte[file.Length];
            await file.OpenReadStream().ReadExactlyAsync(buffer);
            return buffer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string projectId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllPagedAsync(options, i => i.ProjectId == projectId);
            return Ok(mapper.Map<Paged<DocumentBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await application.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(mapper.Map<DocumentDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string projectId, [FromForm] DocumentRequest request)
        {
            try
            {
                var content = await ReadFile(request.File);
                if (content is null) return BadRequest(new ErrorDto("File is empty or null"));

                var result = await application.CreateAsync(
                    new DocumentDomain()
                    {
                        Name = request.File.FileName,
                        Metadata = new() { MimeType = request.File.ContentType, Length = request.File.Length },
                        Content = content,
                        ProjectId = projectId
                    });
                return Created(string.Empty, mapper.Map<DocumentBaseDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string projectId, string id, [FromForm] DocumentRequest request)
        {
            try
            {
                var content = await ReadFile(request.File);
                if (content is null) return BadRequest(new ErrorDto("File is empty or null"));

                var result = await application.UpdateAsync(
                     new DocumentDomain()
                     {
                         Id = id,
                         Name = request.File.FileName,
                         Metadata = new() { MimeType = request.File.ContentType, Length = request.File.Length },
                         Content = content,
                         ProjectId = projectId
                     });

                if (result is null) return NotFound();

                return Ok(mapper.Map<DocumentBaseDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await application.DeleteAsync(id);
            if (result is null) return NotFound();
            return Ok(mapper.Map<DocumentBaseDto>(result));
        }
    }
}
