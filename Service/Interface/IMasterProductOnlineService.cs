using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IMasterProductOnlineService
    {
        Task<ResponseDTO<List<MasterProductOnline>>> Gets();
        Task<ResponseDTO<List<MasterProductOnline>>> Options();
        Task<ResponseDTO<List<ProductTransactionResult>>> GetWithDetail();
        Task<ResponseDTO<List<WarehouseTransactionCheckOutRequest>>> GetCheckDetail(string itemCode,string colorCode,string size);
        Task<ResponseDTO<MasterProductOnline>> Get(string keyword);
        Task<ResponseDTO<string>> GetUOM(string itemCode, string colorCode,string size);
        Task<ResponseDTO<List<ProductTransactionResult>>> GetOutDetail();
    }
}
