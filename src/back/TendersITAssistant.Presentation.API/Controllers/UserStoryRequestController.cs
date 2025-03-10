﻿using AutoMapper;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Project.Group;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TendersITAssistant.Presentation.API;
using TendersITAssistant.Presentation.API.Controllers.Dto;

namespace TendersITAssistant.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/project/{projectId}/group/{groupId}/request")]
    public class UserStoryRequestController(IApplication<UserStoryRequestDomain> application, IMapper mapper)
        : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByParentAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await application.GetByIdAsync(id, cancellationToken);
            if (result is null) return NotFound();
            return Ok(mapper.Map<UserStoryRequestDto>(result));
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateAsync([FromBody] UserStoryRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await application.UpdateAsync(mapper.Map<UserStoryRequestDomain>(request), cancellationToken);
            return result ? Ok() : NotFound();
        }
    }
}
