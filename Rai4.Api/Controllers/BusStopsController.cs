using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rai4.Application.Dto.BusStop;
using Rai4.Application.Dto.Json;
using Rai4.Application.Features.BusStopFeatures.Commands;
using Rai4.Application.Features.BusStopFeatures.Queries;
using Rai4.Application.Services.Interfaces;

namespace Rai4.Api.Controllers;

/// <summary>
/// Bus Stop management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BusStopsController(
    IMediator mediator,
    IZtmClient ztmClient) : ControllerBase
{
    /// <summary>
    /// Gets all bus stops
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of bus stops</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusStopDto>>> GetAllBusStops(
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllBusStopsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Gets all ZTM bus stops from external service
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<FriendlyStop>>> GetAllZtmBusStops(
        CancellationToken cancellationToken = default)
    {
        var result = await ztmClient.GetAllBusStopsAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific bus stop by ID
    /// </summary>
    /// <param name="id">Bus stop ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Bus stop details</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BusStopDto>> GetBusStopById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBusStopByIdQuery { Id = id };
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound($"Bus stop with ID {id} not found");

        return Ok(result);
    }

    /// <summary>
    /// Creates a new bus stop
    /// </summary>
    /// <param name="dto">Bus stop creation data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created bus stop</returns>
    [HttpPost]
    public async Task<ActionResult<BusStopDto>> CreateBusStop(
        [FromBody] CreateBusStopDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateBusStopCommand
        {
            Name = dto.Name,
            ZtmStopId = dto.ZtmStopId
        };
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetBusStopById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing bus stop
    /// </summary>
    /// <param name="id">Bus stop ID</param>
    /// <param name="dto">Bus stop update data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated bus stop</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<BusStopDto>> UpdateBusStop(
        int id,
        [FromBody] UpdateBusStopDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateBusStopCommand
        {
            Id = id,
            Name = dto.Name,
            ZtmStopId = dto.ZtmStopId
        };
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a bus stop
    /// </summary>
    /// <param name="id">Bus stop ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBusStop(
        int id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBusStopCommand { Id = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}