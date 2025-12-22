using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Helper;
using RFIDApi.Models.Context;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;

namespace RFIDApi.Service.FPSService
{
    public class PODetailService : IPODetailService
    {
        private readonly FPSDbContext _context;
        public PODetailService(FPSDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO<List<Purchase_PODetail>>> Gets()
        {
            try
            {
               var res =  await _context.purchase_PODetails.ToListAsync();

               return ResponseFactory<List<Purchase_PODetail>>.Ok("Success",res);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseDTO<Purchase_PODetail>> GetById (string poNo)
        {
            try
            {
                var res = await _context.purchase_PODetails.FirstOrDefaultAsync(x => x.PONo == poNo);
                if (res == null)
                {
                    return ResponseFactory<Purchase_PODetail>.Failed("Not Found Po Number");
                }
                return ResponseFactory<Purchase_PODetail>.Ok("Success", res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ResponseDTO<List<PODetailDTO>>> GetPODetailByPOno(string POno)
        {
            try
            {
                var res = await (
                    from a in _context.purchase_PODetails
                    join b in _context.masterProductOnlines
                    on new { a.ItemCode, a.ColorCode ,a.Size}
                    equals new { b.ItemCode, b.ColorCode ,b.Size}
                    where a.PONo == POno
                    group new { a, b }
                        by new { a.ItemCode, a.ItemNo, a.ColorCode, b.SKU, b.ProductBarcode, a.UOM , a.Size}
                    into g
                    select new PODetailDTO
                    {
                        ItemCode = g.Key.ItemCode,
                        ItemNo = g.Key.ItemNo,
                        ColorCode = g.Key.ColorCode,
                        SKU = g.Key.SKU,
                        ProductBarcode = g.Key.ProductBarcode,
                        UOM = g.Key.UOM,
                        size = g.Key.Size,
                        POQty = g.Sum(x => x.a.POQty) ?? 0,
                        BalanceQty = g.Sum(x => x.a.POQty) - g.Sum(x => x.a.ReceiveQty) ?? 0
                    }
                    ).ToListAsync();


                return ResponseFactory<List<PODetailDTO>>.Ok("Success",res);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
