using AutoMapper;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Common;
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
    public class UserStoryController(UserStoryApplication application, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string groupId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllAsync(options, us => us.GroupId == groupId);
            return Ok(mapper.Map<Paged<UserStoryBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string groupId, string id)
        {
            var result = await application.GetByIdAsync(id);
            if (result is null || result.GroupId != groupId) return NotFound();
            return Ok(mapper.Map<UserStoryDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string groupId, [FromBody] UserStoryDto request)
        {
            try
            {
                var item = mapper.Map<UserStoryDomain>(request);
                item.GroupId = groupId;
                var result = await application.CreateAsync(item);
                return Created(string.Empty, mapper.Map<UserStoryDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(string groupId, [FromBody] UserStoryDto request)
        {
            try
            {
                var item = mapper.Map<UserStoryDomain>(request);
                item.GroupId = groupId;
                var result = await application.UpdateAsync(item);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryDto>(result));
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
            return Ok(mapper.Map<UserStoryDto>(result));
        }
    }
}
