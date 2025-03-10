﻿using AutoMapper;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Presentation.API.Controllers.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TendersITAssistant.Presentation.API;
using TendersITAssistant.Presentation.API.Controllers.Dto;

namespace TendersITAssistant.Presentation.API.Controllers
{
    [EnableCors(PolicyName = ConfigureService.SpaCors)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IApplication<ProjectDomain> application, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken, [FromQuery] int offset = PaginationOptions.DefaultOffset, [FromQuery] int limit = PaginationOptions.DefaultLimit)
        {
            var options = new PaginationOptions(offset, limit);
            var result = await application.GetAllPagedAsync(options, null, cancellationToken);
            return Ok(mapper.Map<Paged<ProjectBaseDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await application.GetByIdAsync(id, cancellationToken);
            if (result is null) return NotFound();
            return Ok(mapper.Map<ProjectDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectBaseDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await application.CreateAsync(mapper.Map<ProjectDomain>(request), cancellationToken);
                return Created(string.Empty, mapper.Map<ProjectDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] ProjectDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await application.UpdateAsync(mapper.Map<ProjectDomain>(request), cancellationToken);
                return result ? Ok() : NotFound();
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
