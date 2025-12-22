using RFIDApi.DTO;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IWarehouseTransactionService
    {
        Task<ResponseDTO<List<FPSWarehouseTransection>>> Gets();
        Task<ResponseDTO<FPSWarehouseTransection>> Get(string keyword);
        
    }
}
