using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Helper;
using RFIDApi.Models;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;

namespace RFIDApi.Service.FPSService
{
    public class MasterProductOnlineService : IMasterProductOnlineService
    {
        private readonly FPSDbContext _fpsContext;
        private readonly RFIDDbContext _shopifyContext;

        public MasterProductOnlineService(FPSDbContext fpsContext, RFIDDbContext shopifyContext)
        {
            _fpsContext = fpsContext;
            _shopifyContext = shopifyContext;
        }

        public async Task<ResponseDTO<List<MasterProductOnline>>> Gets()
        {
            try
            {
                var res = await _fpsContext.masterProductOnlines.ToListAsync();

                return ResponseFactory<List<MasterProductOnline>>.Ok("Success",res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<MasterProductOnline>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<List<ProductTransactionResult>>> GetWithDetail()
        {
            try
            {
                var result = await (
                    from a in _fpsContext.warehouseTransections
                    join b in _fpsContext.warehouseRFIDs
                        on a.RFId equals b.RFID into bb
                    from b in bb.DefaultIfEmpty()

                    join c in _fpsContext.warehouseReceives
                        on a.ReceiveNo equals c.ReceiveNo into cc
                    from c in cc.DefaultIfEmpty()

                    join d in _fpsContext.masterProductOnlines
                        on new { b.ItemCode, b.ColorCode, b.Size, CompanyCode = "FPSTH" }
                        equals new { d.ItemCode, d.ColorCode, d.Size, d.CompanyCode }
                        into dd
                    from d in dd.DefaultIfEmpty()

                    orderby b.ItemCode, b.ColorCode, b.Size, a.RFId

                    select new ProductTransactionResult
                    {
                        Warehouse = a.Warehouse,
                        RFId = a.RFId,
                        ProductBarcode = d.ProductBarcode,
                        SKU = b.SKU,
                        ItemCode = b.ItemCode,
                        ColorCode = b.ColorCode,
                        Size = b.Size,
                        ReceiveNo = a.ReceiveNo,
                        ReceiveDate = a.ReceiveDate,
                        InType = a.InType,
                        InvoiceNo = c.InvoiceNo,
                        InvoiceDate = c.InvoiceDate,
                        PONo = b.PONo,
                        UOM = b.UOM,
                        OutStatus = a.OutStatus,
                        OutDate = a.OutDate,
                        OutNo = a.OutNo,
                        InputBy = c.InputBy,
                        InputDate = c.InputDate
                    }
                ).ToListAsync();

                return ResponseFactory<List<ProductTransactionResult>>.Ok("Success", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<ProductTransactionResult>>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<MasterProductOnline>> Get(string warehouse)
        {
            try
            {
                return ResponseFactory<MasterProductOnline>.Ok("Success");
            }
            catch (Exception ex)
            {
                return ResponseFactory<MasterProductOnline>.Failed(ex.Message);
            }
        }
    }
}
