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
    public async Task<ActionResult<List<Airport>>> GetAirports()
    {
        return await _services.GetAirportsAsync();
    }
    
    [HttpGet("flights/{id}")]
    public async Task<ActionResult<Flight>> GetFlight(int id)
    {
        return await _services.GetFlightByIdAsync(id);
    }

    [HttpPost("flights/search")]
    public async Task AddSearch(SearchFlightRequest request)
    {
        
    }
}