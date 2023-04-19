using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[ApiController]
[Route("admin-api")]
public class FlightsController : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Policy = "BasicAuthentication")]
    public async Task<IActionResult> GetFlight(int id)
    {
            return Ok();
    }
}