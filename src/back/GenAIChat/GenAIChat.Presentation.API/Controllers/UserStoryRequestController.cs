using AutoMapper;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Presentation.API.Controllers.Dto;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/group/{groupId}/request")]
    public class UserStoryRequestController(IMediator mediator, IMapper mapper)
        : ControllerBase
    {

        [HttpGet()]
        public async Task<IActionResult> GetByIdAsync(string groupId, CancellationToken cancellationToken)
        {
            var result = (
                await mediator.Send(new GetAllQuery<UserStoryRequestDomain>()
                {
                    Filter = new PropertyEqualsFilter(nameof(UserStoryRequestDomain.GroupId), groupId)
                }, cancellationToken))
                .FirstOrDefault();
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryRequestDto>(result));
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(string groupId, [FromBody] UserStoryRequestDto request, CancellationToken cancellationToken)
        {
            var result = (
                await mediator.Send(new UpdateCommand<UserStoryRequestDomain>()
                {
                    Filter = new PropertyEqualsFilter(nameof(UserStoryRequestDomain.GroupId), groupId)
                }, cancellationToken))
                .FirstOrDefault();
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryRequestDto>(result));
        }
    }
}
