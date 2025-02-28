using AutoMapper;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/group/{groupId}/[controller]")]
    public class UserStoryController(IApplication<UserStoryDomain> application, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string groupId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit, CancellationToken cancellationToken = default)
        {
            var options = new PaginationOptions(offset, limit);
            var filter = new PropertyEqualsFilter(nameof(UserStoryDomain.GroupId), groupId);
            var result = await application.GetAllPagedAsync(options, filter, cancellationToken);
            return Ok(mapper.Map<Paged<UserStoryBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string groupId, string id, CancellationToken cancellationToken = default)
        {
            var result = await application.GetByIdAsync(id, cancellationToken);
            if (result is null || result.GroupId != groupId) return NotFound();
            return Ok(mapper.Map<UserStoryDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string groupId, [FromBody] UserStoryDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var domain = mapper.Map<UserStoryDomain>(request);
                domain.GroupId = groupId;
                var result = await application.CreateAsync(domain, cancellationToken);
                return Created(string.Empty, mapper.Map<UserStoryDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(string groupId, [FromBody] UserStoryDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var domain = mapper.Map<UserStoryDomain>(request);
                domain.GroupId = groupId;
                var result = await application.UpdateAsync(domain, cancellationToken);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await application.DeleteAsync(id, cancellationToken);
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryDto>(result));
        }
    }
}
