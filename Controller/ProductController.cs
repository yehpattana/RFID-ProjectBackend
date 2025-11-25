using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RFIDApi.Models;
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
namespace RFIDApi.controller
{
    [Route("rfidApi/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly RFIDDbContext _context;
        private readonly IHubContext<RFIDHubs> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        public ProductController(RFIDDbContext db, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
        {
            _context = db;
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _scopeFactory = scopeFactory;
        }

        [HttpGet("GetProductData")]
        public async Task<IActionResult> GetProductData()
        {
            try
            {
                var data = await _context.Products.Where(t => t.Barcode != null && t.Barcode.Trim() != "").ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllProductRFID")]
        public async Task<IActionResult> GetProductRFID()
        {
            try
            {
                var data = await _context.ProductsRFID.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("GetProductRFID")]
        public async Task<IActionResult> GetProductRFID([FromQuery] string EPC)
        {
            try
            {
                var data = await _context.ProductsRFID.FirstOrDefaultAsync(t => t.RFID == EPC);
                return Ok(data);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("AddRfidToProduct")]
        public async Task<IActionResult> AddRfidToProduct(AddRfidToProductRequest[] request)
        {
            try
            {
                if(request == null)
                {
                    return BadRequest("Request Data is Empty");
                }
                var error = new List<string>();
                foreach (var item in request) {
                    
                    var db = await _context.ProductsRFID.Where(t => t.RFID == item.EPC).ToListAsync();
                    if(db.Count > 0 || db.Any())
                    {
                        foreach(var i in db)
                        {
                            error.Add($"{i.RFID} is Already Register on {i.SKU}");
                        }
                    }
                    else
                    {

                        var data = await _context.Products.FirstOrDefaultAsync(t => t.Barcode == item.Barcode);
                        var newData = new ProductRFID
                        {
                            RFID = item.EPC,
                            SKU = data.Sku,
                            CreateDate = DateTime.Now
                        };
                        await _context.ProductsRFID.AddAsync(newData);
                        
                    }
                }
                if (error.Any())
                {
                    return BadRequest(error);
                }
                await _context.SaveChangesAsync();
                return Ok("Add RFID Success");
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddRfidToProductBySKU")]
        public async Task<IActionResult> AddRfidToProductBySKU(AddRfidToProductBySKURequest[] request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request Data is Empty");
                }

                var error = new List<string>();
                foreach (var item in request)
                {

                    var db = await _context.ProductsRFID.Where(t => t.RFID == item.EPC).ToListAsync();
                    if (db.Count > 0 || db.Any())
                    {
                        foreach (var i in db)
                        {
                            error.Add($"{i.RFID} is Already Register on {i.SKU}");
                        }
                    }
                    else
                    {

                        var data = await _context.Products.FirstOrDefaultAsync(t => t.Sku == item.SKU);
                        var newData = new ProductRFID
                        {
                            RFID = item.EPC,
                            SKU = data.Sku,
                            CreateDate = DateTime.Now
                        };
                        await _context.ProductsRFID.AddAsync(newData);

                    }
                }
                if (error.Any())
                {
                    return BadRequest(error);
                }
                await _context.SaveChangesAsync();
                return Ok("Add RFID Success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteProductRFID")]
        public async Task<IActionResult> DeleteProductRFID(DeleteRfidProductRequest[] data)
        {
            try
            {
                var rfids = data.Select(d => d.rfid).ToList();

                var itemsToDelete = await _context.ProductsRFID
                            .ToListAsync();

                if (!itemsToDelete.Any())
                {
                    return BadRequest("Not found Product in system");
                }
                var existItem = itemsToDelete.Where(p => rfids.Contains(p.RFID)).ToList();
                _context.ProductsRFID.RemoveRange(existItem);
                await _context.SaveChangesAsync();
                
                return Ok($"Remove RFID Product success Total : {existItem.Count}");

            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}
