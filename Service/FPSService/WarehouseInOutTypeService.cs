using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.Helper;
using RFIDApi.Models;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;

namespace RFIDApi.Service.FPSService
{
    public class WarehouseInOutTypeService : IWarehouseInOutTypeService
    {
        private readonly FPSDbContext _fpsContext;
        private readonly RFIDDbContext _shopifyContext;
        public WarehouseInOutTypeService(FPSDbContext fpsContext, RFIDDbContext shopifyContext)
        {
            _fpsContext = fpsContext;
            _shopifyContext = shopifyContext;
        }

        public async Task<ResponseDTO<List<WarehouseInOutType>>> Gets()
        {
            try
            {
                var res = await _fpsContext.warehouseInOutTypes.ToListAsync();

                return ResponseFactory<List<WarehouseInOutType>>.Ok("Success",res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<WarehouseInOutType>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<WarehouseInOutType>> Get(string warehouse)
        {
            try
            {
                return ResponseFactory<WarehouseInOutType>.Ok("Success");
            }
            catch (Exception ex)
            {
                return ResponseFactory<WarehouseInOutType>.Failed(ex.Message);
            }
        }
    }
}
