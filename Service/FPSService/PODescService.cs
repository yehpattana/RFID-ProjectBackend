using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.Helper;
using RFIDApi.Models.Context;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;

namespace RFIDApi.Service.FPSService
{
    public class PODescService : IPODescService
    {
        private readonly FPSDbContext _context;
        public PODescService(FPSDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO<List<Purchase_PODesc>>> Gets()
        {
            try
            {
               var res =  await _context.purchase_PODescs.ToListAsync();

               return ResponseFactory<List<Purchase_PODesc>>.Ok("Success",res);
            }
            catch(Exception ex)
            {
                return ResponseFactory<List<Purchase_PODesc>>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<Purchase_PODesc>>> Options()
        {
            try
            {
                var res = await _context.purchase_PODescs.Where(t => !t.CancelStatus && t.ApprovePO).ToListAsync();
                return ResponseFactory<List<Purchase_PODesc>>.Ok("Success", res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<Purchase_PODesc>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<Purchase_PODesc>> GetById (string poNo)
        {
            try
            {
                var res = await _context.purchase_PODescs.FirstOrDefaultAsync(x => x.PONo == poNo);
                if (res == null)
                {
                    return ResponseFactory<Purchase_PODesc>.Failed("Not Found Po Number");
                }
                return ResponseFactory<Purchase_PODesc>.Ok("Success", res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Purchase_PODesc>.Failed(ex.Message);
            }
        }

    }
}
