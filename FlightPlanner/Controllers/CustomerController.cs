using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[Route("api")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly DbServices _services;

    public CustomerController(DbServices services)
    {
        _services = services;
    }
    
    [HttpGet("airports")]
    public async Task<List<Airport>> GetAirports(string search)
    {
        return await _services.GetAirportsAsync(search);
    }

    [HttpPost("flights/search")]
    public async Task<ActionResult<FlightSearchRequest>> GetFlightsByRequest(SearchFlightRequest request)
    {
        if (request.From == request.To)
        {
            return BadRequest();
        }
        var flights = await _services.GetFlightsBySearchAsync(request);
        var response = new FlightSearchRequest
        {
            Items = flights,
            Page = 0,
            TotalItems = flights.Count
        };
        return response;
    }

    [HttpGet("flights/{id}")]
    public async Task<ActionResult<Flight>> GetFlightById(int id)
    {
        var flight = await _services.GetFlightByIdAsync(id);
        return flight == null ? NotFound() : flight;
    }

}