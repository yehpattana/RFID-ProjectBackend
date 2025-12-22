using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.Helper;
using RFIDApi.Models;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;

namespace RFIDApi.Service.FPSService
{
    public class WarehouseTransactionService : IWarehouseTransactionService
    {
        private readonly FPSDbContext _fpsContext;
        private readonly RFIDDbContext _shopifyContext;
        public WarehouseTransactionService(FPSDbContext fpsContext, RFIDDbContext shopifyContext)
        {
            _fpsContext = fpsContext;
            _shopifyContext = shopifyContext;
        }

        public async Task<ResponseDTO<List<FPSWarehouseTransection>>> Gets()
        {
            try
            {
                var res = await _fpsContext.warehouseTransections.ToListAsync();

                return ResponseFactory<List<FPSWarehouseTransection>>.Ok("Success",res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<FPSWarehouseTransection>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<FPSWarehouseTransection>> Get(string warehouse)
        {
            try
            {
                return ResponseFactory<FPSWarehouseTransection>.Ok("Success");
            }
            catch (Exception ex)
            {
                return ResponseFactory<FPSWarehouseTransection>.Failed(ex.Message);
            }
        }
    }
}
