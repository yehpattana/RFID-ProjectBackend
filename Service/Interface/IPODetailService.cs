using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IPODetailService
    {
        Task<ResponseDTO<List<Purchase_PODetail>>> Gets();
        Task<ResponseDTO<Purchase_PODetail>> GetById(string poNo);
        Task<ResponseDTO<List<PODetailDTO>>> GetPODetailByPOno(string POno);
    }
}
