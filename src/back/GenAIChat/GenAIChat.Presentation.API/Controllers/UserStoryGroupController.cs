using AutoMapper;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/group")]
    public class UserStoryGroupController(IUserStoryGroupApplication application, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string projectId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit, CancellationToken cancellationToken = default)
        {
            var options = new PaginationOptions(offset, limit);
            var filter = new PropertyEqualsFilter(nameof(UserStoryGroupDomain.ProjectId), projectId);
            return Ok(mapper.Map<Paged<UserStoryGroupBaseDto>>(await application.GetAllPagedAsync(options, filter, cancellationToken)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string projectId, string id, CancellationToken cancellationToken = default)
        {
            var result = await application.GetByIdAsync(id, cancellationToken);
            if (result is null || result.ProjectId != projectId) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string projectId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await application.CreateAsync(projectId, cancellationToken);
                var map = mapper.Map<UserStoryGroupDto>(result);
                return Created(string.Empty, mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(string projectId, [FromBody] UserStoryGroupDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                domain.ProjectId = projectId;
                var result = await application.UpdateAsync(domain, cancellationToken);

                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/generate")]
        public async Task<IActionResult> Generate(string projectId, string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await application.GenerateUserStoriesAsync(projectId, id, cancellationToken);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/validate")]
        public async Task<IActionResult> Validate(string projectId, string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await application.ValidateCostsAsync(projectId, id, cancellationToken);
                return Ok(mapper.Map<UserStoryGroupDto>(result));
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
            return result ? Ok() : NotFound();
        }
    }
}
