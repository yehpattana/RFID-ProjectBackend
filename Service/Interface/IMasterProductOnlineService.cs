using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IMasterProductOnlineService
    {
        Task<ResponseDTO<List<MasterProductOnline>>> Gets();
        Task<ResponseDTO<List<ProductTransactionResult>>> GetWithDetail();
        Task<ResponseDTO<MasterProductOnline>> Get(string keyword);


        
    }
}
