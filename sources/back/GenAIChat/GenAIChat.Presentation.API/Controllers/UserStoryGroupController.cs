using AutoMapper;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Common;
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
    public class UserStoryGroupController(UserStoryGroupApplication application, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int projectId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllAsync(options, group => group.ProjectId == projectId);
            return Ok(mapper.Map<Paged<UserStoryGroupBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int projectId, int id)
        {
            var result = await application.GetByIdAsync(id);
            if (result is null || result.ProjectId != projectId) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(int projectId)
        {
            try
            {
                var result = await application.CreateAsync(projectId);
                var map = mapper.Map<UserStoryGroupDto>(result);
                return Created(string.Empty, mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(int projectId, [FromBody] UserStoryGroupDto request)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                domain.ProjectId = projectId;
                var result = await application.UpdateAsync(domain);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/request")]
        public async Task<IActionResult> UpdateRequest(int projectId, int id, [FromBody] UserStoryGroupDto request)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                if (domain.Id != id) return BadRequest();

                domain.ProjectId = projectId;
                var result = await application.UpdateRequestAsync(domain.Id, domain.Request);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/story")]
        public async Task<IActionResult> UpdateUserStories(int projectId, int id, [FromBody] UserStoryGroupDto request)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                if (domain.Id != id) return BadRequest(); 
                
                domain.ProjectId = projectId;
                var result = await application.UpdateUserStoriesAsync(domain.Id, domain.UserStories);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/generate")]
        public async Task<IActionResult> Generate(int projectId, int id)
        {
            try
            {
                var result = await application.GenerateAsync(projectId, id);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/validate")]
        public async Task<IActionResult> Validate(int projectId, int id)
        {
            try
            {
                var result = await application.Validate(projectId, id);
                return Ok(mapper.Map<UserStoryGroupDto>(result));
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
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }
    }
}
