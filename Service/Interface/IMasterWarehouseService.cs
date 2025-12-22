using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;

namespace RFIDApi.Service.Interface
{
    public interface IMasterWarehouseService
    {
        Task<ResponseDTO<List<MasterWarehouse>>> Gets();

        Task<ResponseDTO<List<MasterWarehouse>>> Options(string company);
        Task<ResponseDTO<MasterWarehouse>> Get(string warehouse);
        Task<ResponseDTO<List<WarehouseReceiveInDTO>>> GetReceiveIns(string ReceiveNo);

        Task<ResponseDTO<List<WarehouseRFID>>> GetWarehouseRFID ();
        Task<ResponseDTO<string>> AutoRunReceiveInNo(string company);
        Task<ResponseDTO<object>> CreateWarehouseReceive(CreateWarehouseReceiveInDTO req);
        Task<ResponseDTO<object>> UpdateWarehouseReceive(string receiveNo, CreateWarehouseReceiveInDTO req);

        Task<ResponseDTO<object>> DeleteWarehouseReceive(string receiveNo); 

    }
}
