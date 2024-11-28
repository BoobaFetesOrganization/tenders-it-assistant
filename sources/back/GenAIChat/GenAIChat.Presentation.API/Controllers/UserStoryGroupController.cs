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
        public async Task<IActionResult> Create(int projectId, [FromBody] UserStoryGroupDto request)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                domain.ProjectId = projectId;
                var result = await application.CreateAsync(domain);
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

        [HttpPut("generate")]
        public async Task<IActionResult> Generate(int projectId, [FromBody] UserStoryPromptDomain prompt)
        {
            try
            {
                var result = await application.Generate(projectId, prompt);
                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("validate")]
        public async Task<IActionResult> Validate(int projectId, [FromBody] UserStoryGroupDto request)
        {
            try
            {

                var result = await application.Validate(projectId, request.Id);
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
            var result = await application.DeleteAsync(id); ;
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }
    }
}
