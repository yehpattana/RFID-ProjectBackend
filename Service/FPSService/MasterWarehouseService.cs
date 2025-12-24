using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO;
using RFIDApi.DTO.Data;
using RFIDApi.Helper;
using RFIDApi.Models.FPS;
using RFIDApi.Service.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.Models.Context;

namespace RFIDApi.Service.FPSService
{
    public class MasterWarehouseService : IMasterWarehouseService
    {
        private readonly FPSDbContext _fpsContext;
        private readonly RFIDDbContext _shopifyContext;
        public MasterWarehouseService(FPSDbContext fpsContext, RFIDDbContext shopifyContext)
        {
            _fpsContext = fpsContext;
            _shopifyContext = shopifyContext;
        }

        public async Task<ResponseDTO<List<MasterWarehouse>>> Gets()
        {
            try
            {
                var res = await _fpsContext.masterWarehouses.ToListAsync();

                return ResponseFactory<List<MasterWarehouse>>.Ok("Success", res);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<MasterWarehouse>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<List<MasterWarehouse>>> Options(string company)
        {
            try
            {
                var res = await (from a in _fpsContext.masterWarehouses
                                 where a.CompanyCode == company && !a.Cancel
                                 select a).ToListAsync();

                return ResponseFactory<List<MasterWarehouse>>.Ok("Success", res);

            }
            catch (Exception ex)
            {
                return ResponseFactory<List<MasterWarehouse>>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<MasterWarehouse>> Get(string warehouse)
        {
            try
            {
                return ResponseFactory<MasterWarehouse>.Ok("Success");
            }
            catch (Exception ex)
            {
                return ResponseFactory<MasterWarehouse>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<List<WarehouseRFID>>> GetWarehouseRFID()
        {
            try
            {
                var res = await _fpsContext.warehouseRFIDs.ToListAsync();


                return ResponseFactory<List<WarehouseRFID>>.Ok("Success", res);
                
            }
            catch (Exception ex) 
            { 
                return ResponseFactory<List<WarehouseRFID>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> AutoRunReceiveInNo(string company)
        {
            try
            {
                var result = _fpsContext.Set<GenReceiveNoResultDTO>()
                    .FromSqlRaw("EXEC sp_GenInventoryDocNo {0}, {1}", company, "IN")
                    .AsEnumerable()  // ← บังคับให้ผลลัพธ์มาอยู่ใน memory ก่อน
                    .FirstOrDefault();


                return ResponseFactory<string>.Ok("Success", result.DocNo);
            }
            catch (Exception ex)
            {
                return ResponseFactory<string>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<object>> CreateWarehouseReceive(CreateWarehouseReceiveInDTO req)
        {
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {

                if(req == null)
                {
                    return ResponseFactory<object>.Failed("Request is null");
                }

                var newData = new WarehouseReceive
                {
                    ReceiveNo = req.receiveNo,
                    CompanyCode = req.companyCode,
                    Warehouse = req.warehouse,
                    ReceiveDate = req.receiveDate,
                    ReceiveType = req.receiveType,
                    DeliveryNo = req.deliveryNo,
                    InvoiceNo = req.invoiceNo,
                    InvoiceDate = req.invoiceDate,
                    InputBy = req.createdBy,
                    InputDate = DateTime.Now,
                    EditBy = req.createdBy,
                    EditDate = DateTime.Now,
                    Remark = req.remark,
                };

                var res = await _fpsContext.warehouseReceives.AddAsync(newData);

                List<FPSWarehouseTransection> tranList = new List<FPSWarehouseTransection>();
                foreach(var item in req.rfidlist)
                {
                    //@RFId,
                    //@ReceiveNo,
                    //@PONo,
                    //@POItemNo,
                    //@UOM,
                    //@Company,
                    //@ItemCode,
                    //@ColorCode,
                    //@Size
                    if(newData.ReceiveType == "Purchase")
                    {
                        var sql = @"
                        EXEC sp_WarehouseRegisterRFId 
                        {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}
                        ";

                        _fpsContext.Database.ExecuteSqlRaw(
                            sql,
                            item.rfid,
                            newData.ReceiveNo,
                            item.poNo,
                            item.poNoItem,
                            item.uom,
                            newData.CompanyCode,
                            item.itemCode,
                            item.colorCode,
                            item.size
                        );
                    }


                    var transactionNew = new FPSWarehouseTransection
                    {
                        ReceiveNo = newData.ReceiveNo,
                        Warehouse = newData.Warehouse,
                        RFId = item.rfid,
                        CompanyCode = newData.CompanyCode,
                        PONo = item.poNo,
                        POItemNo = item.poNoItem,
                        UOM = item.uom,
                        InType = newData.ReceiveType,
                        ReceiveDate = newData.ReceiveDate,
                        OutStatus = false,
                        InputBy = newData.InputBy,
                        InputDate = DateTime.Now,
                    };
                    tranList.Add(transactionNew);
                }
                await _fpsContext.warehouseTransections.AddRangeAsync(tranList);

                await _fpsContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ResponseFactory<object>.Ok("Success");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseFactory<object>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<object>> UpdateWarehouseReceive(string receiveNo, CreateWarehouseReceiveInDTO req)
        {
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {
                var existData = await _fpsContext.warehouseReceives.FirstOrDefaultAsync(t => t.ReceiveNo == receiveNo);

                if(existData == null)
                {
                    return ResponseFactory<object>.Failed("Not found receive No");
                }

                existData.ReceiveDate = req.receiveDate;
                existData.DeliveryNo = req.deliveryNo;
                existData.InvoiceNo = req.invoiceNo ?? null;
                existData.InvoiceDate = req.invoiceDate ?? null;
                existData.Remark = req.remark ?? null;
                existData.Warehouse = req.warehouse;
                existData.EditBy = req.createdBy ?? "system";
                existData.EditDate = DateTime.Now;

                foreach(var item in req.rfidlist)
                {
                    var exist = await _fpsContext.warehouseTransections.FirstOrDefaultAsync(t => t.RFId == item.rfid && t.ReceiveNo == existData.ReceiveNo);
                    if (exist != null)
                    {
                        exist.Warehouse = existData.Warehouse;
                        exist.ReceiveDate = DateTime.Now;
                    }
                }

                var sql = @"
                          EXEC sp_UpdatePOReceive
                          {0}
                          ";

                _fpsContext.Database.ExecuteSqlRaw(sql, receiveNo);

                await _fpsContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ResponseFactory<object>.Ok("Success");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseFactory<object>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<object>> DeleteWarehouseReceive(string receiveNo)
        {
            try
            {
                var existRec = await _fpsContext.warehouseReceives.Where(t => t.ReceiveNo.Equals(receiveNo)).FirstOrDefaultAsync();
                var existTran = await _fpsContext.warehouseTransections.Where(t => t.ReceiveNo.Equals(receiveNo)).ToListAsync();

                if (existRec == null) {
                    return ResponseFactory<object>.Failed("Not found receive packing list data");
                }
                var hasOutRfid = await _fpsContext.warehouseTransections
                    .AnyAsync(t => t.ReceiveNo == receiveNo && t.OutStatus);
                if (hasOutRfid) {
                    return ResponseFactory<object>.Failed("Have rfid is out from stock");
                }
                _fpsContext.RemoveRange(existRec);
                _fpsContext.RemoveRange(existTran);

                if(existRec.ReceiveType == "Purchase"){
                    var sql = @"
                            EXEC 
                            sp_WarehouseDeleteRFId 
                            {0}";

                    _fpsContext.Database.ExecuteSqlRaw(sql, receiveNo);
                }


                await _fpsContext.SaveChangesAsync();

                return ResponseFactory<object>.Ok("Success");
            }
            catch (Exception ex)
            {
                return ResponseFactory<object>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<WarehouseReceiveInDTO>>> GetReceiveIns(string ReceiveNo) 
        {
            try
            {

                var result = await (
                    from r in _fpsContext.warehouseReceives
                    where r.ReceiveNo == ReceiveNo
                    join t in _fpsContext.warehouseTransections
                        on r.ReceiveNo equals t.ReceiveNo into rt
                    from t in rt.DefaultIfEmpty()
                    join rf in _fpsContext.warehouseRFIDs
                        on t.RFId equals rf.RFID into trf
                    from rf in trf.DefaultIfEmpty()
                    select new
                    {
                        Receive = r,
                        Transaction = t,
                        RFID = rf
                    }
                )
                .GroupBy(x => x.Receive.ReceiveNo)
                .Select(g => new WarehouseReceiveInDTO
                {
                    receiveNo = g.Key,
                    receiveDate = g.First().Receive.ReceiveDate.HasValue
                                    ? DateTime.SpecifyKind(g.First().Receive.ReceiveDate.Value, DateTimeKind.Utc)
                                    : null,
                    receiveType = g.First().Receive.ReceiveType,
                    companyCode = g.First().Receive.CompanyCode,
                    deliveryNo = g.First().Receive.DeliveryNo,
                    invoiceNo = g.First().Receive.InvoiceNo,
                    invoiceDate = g.First().Receive.InvoiceDate.HasValue
                                    ? DateTime.SpecifyKind(g.First().Receive.InvoiceDate.Value, DateTimeKind.Utc)
                                    : null,
                    warehouse = g.First().Receive.Warehouse,
                    createdBy = g.First().Receive.InputBy,
                    remark = g.First().Receive.Remark,

                    rfidlist = g
                        .Where(x => x.RFID != null)
                        .Select(x => new RFIDPOList
                        {
                            rfid = x.RFID.RFID,
                            poNo = x.RFID.PONo,
                            poNoItem = x.RFID.POItemNo,
                            itemCode = x.RFID.ItemCode,
                            colorCode = x.RFID.ColorCode,
                            size = x.RFID.Size,
                            uom = x.RFID.UOM,
                            sku = x.RFID.SKU,
                            Status = x.Transaction != null
                            ? x.Transaction.OutStatus
                            : null
                        })
                        .ToList()
                })
                .ToListAsync();


                return ResponseFactory<List<WarehouseReceiveInDTO>>.Ok("Success", result);
            }
            catch (Exception ex) { 
                return ResponseFactory<List<WarehouseReceiveInDTO>>.Failed(ex.Message);
            }
        }
    }
}
