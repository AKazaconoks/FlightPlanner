using FlightPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[ApiController]
[Route("testing-api")]
public class TestingController : ControllerBase
{
    private readonly DbServices _services;

    public TestingController(DbServices services)
    {
        _services = services;
    }

    [HttpPost("clear")]
    public async Task<IActionResult> DeleteFlights()
    {
        await _services.DeleteAllFlightsAsync();
        await _services.DeleteAllAirportsAsync();
        return Ok();
    }
}