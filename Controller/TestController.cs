using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RFIDApi.Hubs;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IHubContext<RFIDHubs> _hubContext;

    public TestController(IHubContext<RFIDHubs> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastTest()
    {
        await _hubContext.Clients.All.SendAsync("TestBroadcast","test");
        return Ok("Broadcast sent");
    }
}