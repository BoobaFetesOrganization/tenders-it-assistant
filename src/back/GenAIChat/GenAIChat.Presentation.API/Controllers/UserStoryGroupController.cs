using AutoMapper;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Command.Project.Group;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Dto;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/group")]
    public class UserStoryGroupController(IMediator mediator, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken, string projectId, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var filter = new PropertyEqualsFilter(nameof(UserStoryGroupDomain.ProjectId), projectId);
            var result = await mediator.Send(new GetAllPagedQuery<UserStoryGroupDomain>() { Options = options }, cancellationToken);
            return Ok(mapper.Map<Paged<UserStoryGroupBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string projectId, string id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = id }, cancellationToken);
            if (result is null || result.ProjectId != projectId) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string projectId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(new CreateCommand<UserStoryGroupDomain>()
                {
                    Domain = new()
                    {
                        ProjectId = projectId
                    }
                }, cancellationToken);
                var map = mapper.Map<UserStoryGroupDto>(result);
                return Created(string.Empty, mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(string projectId, [FromBody] UserStoryGroupDto request, CancellationToken cancellationToken)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                domain.ProjectId = projectId;
                var result = await mediator.Send(new UpdateCommand<UserStoryGroupDomain>()
                {
                    Domain = domain
                }, cancellationToken);
                var result = await application.UpdateAsync(domain);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{groupId}/request")]
        public async Task<IActionResult> UpdateRequest(string groupId, [FromBody] UserStoryRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var domain = mapper.Map<UserStoryRequestDomain>(request);
                if (domain.GroupId != groupId) return BadRequest();

                var group = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = groupId }, cancellationToken);
                if (group is null) return NotFound();

                List<Task> defered = new();
                var userStories = await mediator.Send(new GetAllQuery<UserStoryDomain>()
                {
                    Filter = new PropertyEqualsFilter(nameof(UserStoryDomain.GroupId), groupId)
                }, cancellationToken);
                defered.AddRange(userStories.Select(story => mediator.Send(new DeleteCommand<UserStoryDomain>() { Domain = story }, cancellationToken)));


                group.Response = null;
                group.Request = domain;
                var result = await mediator.Send(new UpdateCommand<UserStoryGroupDomain>() { Domain = group }, cancellationToken);

                // wait for all requests to finish
                await Task.WhenAll(defered);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/story")]
        public async Task<IActionResult> UpdateUserStories(string projectId, string id, [FromBody] UserStoryGroupDto request, CancellationToken cancellationToken)
        {
            try
            {
                var domain = mapper.Map<UserStoryGroupDomain>(request);
                if (domain.Id != id) return BadRequest();

                domain.ProjectId = projectId;
                var result = await mediator.Send(new GetAllPagedQuery<UserStoryGroupDomain>() { Options = options }, cancellationToken);
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
        public async Task<IActionResult> Generate(string projectId, string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(new UserStoryGroupGenerateCommand() { ProjectId = projectId, GroupId = id }, cancellationToken);

                if (result is null) return NotFound();

                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut("{id}/validate")]
        public async Task<IActionResult> Validate(string projectId, string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(new UserStoryGroupValidateCommand() { ProjectId = id, GroupId = id }, cancellationToken);
                return Ok(mapper.Map<UserStoryGroupDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteByIdCommand<UserStoryGroupDomain>() { Id = id }, cancellationToken);
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryGroupDto>(result));
        }
    }
}
