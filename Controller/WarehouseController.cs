using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RFIDReaderAPI;
using RFIDReaderAPI.Interface;
using RFIDReaderAPI.Models;
using RFIDApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;
using RFIDApi.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using RFIDApi.Models.Context;
namespace RFIDApi.controller
{
    [Route("rfidApi/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {

        private readonly RFIDDbContext _context;
        private readonly IHubContext<RFIDHubs> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        public WarehouseController(RFIDDbContext db, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
        {
            _context = db;
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _scopeFactory = scopeFactory;
        }

        [HttpGet("GetWarehouseStock")]
        public async Task<IActionResult> GetWarehouseStock()
        {
            try
            {
                var data = await _context.shopifyWarehouseStocks.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
