using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[Route("admin-api")]
[ApiController, Authorize]
public class AdminApiController : ControllerBase
{
    private readonly DbServices _services;

    public AdminApiController(DbServices services)
    {
        _services = services;
    }

    [HttpGet("flights")]
    public async Task<ActionResult<List<Flight>>> GetFlights()
    {
        return await _services.GetFlightsAsync();
    }

    [HttpGet("flights/{id}")]
    public async Task<ActionResult<Flight>> GetFlight(int id)
    {
        var flight = await _services.GetFlightByIdAsync(id);
        return flight == null ? NotFound() : flight;
    }

    [HttpPut("flights")]
    public async Task<ActionResult<Flight>> AddFlights(Flight flight)
    {
        if (!ModelState.IsValid || !DbServices.CheckIfFlightIsValid(flight))
        {
            return BadRequest();
        }

        flight.From = await _services.FindOrCreateAirportAsync(flight.From);
        flight.To = await _services.FindOrCreateAirportAsync(flight.To);
        var isAlreadyAdded = await _services.FindOrCreateFlightAsync(flight);
        if (isAlreadyAdded)
        {
            return Conflict();
        }

        return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
    }

    [HttpDelete("flights/{id}")]
    public async Task<ActionResult<Flight>> DeleteFlight(int id)
    {
        var flight = await _services.GetFlightByIdAsync(id);
        await _services.DeleteFlightAsync(flight);
        return Ok();
    }
}