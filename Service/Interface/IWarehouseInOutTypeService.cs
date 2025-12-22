using RFIDApi.DTO;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IWarehouseInOutTypeService
    {
        Task<ResponseDTO<List<WarehouseInOutType>>> Gets();
        Task<ResponseDTO<WarehouseInOutType>> Get(string keyword);
        
    }
}
