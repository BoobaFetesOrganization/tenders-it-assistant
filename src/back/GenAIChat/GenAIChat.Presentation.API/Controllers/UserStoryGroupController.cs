using AutoMapper;
using GenAIChat.Application.Usecase;
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
    public class UserStoryGroupController(UserStoryGroupApplication application, IMapper mapper)
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

                return result is null ? NoContent() : result.Value ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        // pas dans le bon controller / pas de raccouri / merci [HttpPut("{groupId}/request")]
        // pas dans le bon controller / pas de raccouri / merci public async Task<IActionResult> UpdateRequest(string groupId, [FromBody] UserStoryRequestDto request, CancellationToken cancellationToken = default)
        // pas dans le bon controller / pas de raccouri / merci {
        // pas dans le bon controller / pas de raccouri / merci     try
        // pas dans le bon controller / pas de raccouri / merci     {
        // pas dans le bon controller / pas de raccouri / merci         throw new Exception("project/group/UpdateUserStories ne devrait pas etre utilisé mais [Put]project/group/request");
        // pas dans le bon controller / pas de raccouri / merci         //var domain = mapper.Map<UserStoryRequestDomain>(request);
        // pas dans le bon controller / pas de raccouri / merci         //if (domain.GroupId != groupId) return BadRequest();
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //var group = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = groupId }, cancellationToken);
        // pas dans le bon controller / pas de raccouri / merci         //if (group is null) return NotFound();
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //List<Task> defered = new();
        // pas dans le bon controller / pas de raccouri / merci         //var userStories = await mediator.Send(new GetAllQuery<UserStoryDomain>()
        // pas dans le bon controller / pas de raccouri / merci         //{
        // pas dans le bon controller / pas de raccouri / merci         //    Filter = new PropertyEqualsFilter(nameof(UserStoryDomain.GroupId), groupId)
        // pas dans le bon controller / pas de raccouri / merci         //}, cancellationToken);
        // pas dans le bon controller / pas de raccouri / merci         //defered.AddRange(userStories.Select(story => mediator.Send(new DeleteCommand<UserStoryDomain>() { Domain = story }, cancellationToken)));
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //group.Response = null;
        // pas dans le bon controller / pas de raccouri / merci         //group.Request = domain;
        // pas dans le bon controller / pas de raccouri / merci         //var result = await application.UpdateAsync(domain,cancellationToken);
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //// wait for all requests to finish
        // pas dans le bon controller / pas de raccouri / merci         //await Task.WhenAll(defered);
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //if (result is null) return NotFound();
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //return Ok(mapper.Map<UserStoryGroupDto>(result));
        // pas dans le bon controller / pas de raccouri / merci     }
        // pas dans le bon controller / pas de raccouri / merci     catch (Exception ex)
        // pas dans le bon controller / pas de raccouri / merci     {
        // pas dans le bon controller / pas de raccouri / merci         return BadRequest(new ErrorDto(ex));
        // pas dans le bon controller / pas de raccouri / merci     }
        // pas dans le bon controller / pas de raccouri / merci }

        // pas dans le bon controller / pas de raccouri / merci [HttpPut("{id}/story")]
        // pas dans le bon controller / pas de raccouri / merci public async Task<IActionResult> UpdateUserStories(string projectId, string id, [FromBody] UserStoryGroupDto request, CancellationToken cancellationToken = default)
        // pas dans le bon controller / pas de raccouri / merci {
        // pas dans le bon controller / pas de raccouri / merci     try
        // pas dans le bon controller / pas de raccouri / merci     {
        // pas dans le bon controller / pas de raccouri / merci         throw new Exception("project/group/UpdateUserStories ne devrait pas etre utilisé mais [Put]project/group/story");
        // pas dans le bon controller / pas de raccouri / merci         //var domain = mapper.Map<UserStoryGroupDomain>(request);
        // pas dans le bon controller / pas de raccouri / merci         //if (domain.Id != id) return BadRequest();
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //domain.ProjectId = projectId;
        // pas dans le bon controller / pas de raccouri / merci         //var result = await mediator.Send(new GetAllPagedQuery<UserStoryGroupDomain>() { Options = options }, cancellationToken);
        // pas dans le bon controller / pas de raccouri / merci         //var result = await application.UpdateUserStoriesAsync(domain.Id, domain.UserStories);
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //if (result is null) return NotFound();
        // pas dans le bon controller / pas de raccouri / merci 
        // pas dans le bon controller / pas de raccouri / merci         //return Ok(mapper.Map<UserStoryGroupDto>(result));
        // pas dans le bon controller / pas de raccouri / merci     }
        // pas dans le bon controller / pas de raccouri / merci     catch (Exception ex)
        // pas dans le bon controller / pas de raccouri / merci     {
        // pas dans le bon controller / pas de raccouri / merci         return BadRequest(new ErrorDto(ex));
        // pas dans le bon controller / pas de raccouri / merci     }
        // pas dans le bon controller / pas de raccouri / merci }

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
            return result is null ? NoContent() : result.Value ? Ok() : NotFound();
        }
    }
}
