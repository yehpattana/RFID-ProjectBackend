using RFIDApi.DTO;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IPODescService
    {
        Task<ResponseDTO<List<Purchase_PODesc>>> Gets();

        Task<ResponseDTO<List<Purchase_PODesc>>> Options();
        Task<ResponseDTO<Purchase_PODesc>> GetById(string poNo);
    }
}
