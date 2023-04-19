using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers;

[ApiController]
[Route("testing-api")]
public class TestingController : ControllerBase
{
    [HttpPost("clear")]
    public async Task<IActionResult> DeleteFlight()
    {
        return Ok();
    }
}