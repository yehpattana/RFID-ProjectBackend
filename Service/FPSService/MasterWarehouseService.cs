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
using System.Security.Claims;

namespace RFIDApi.Service.FPSService
{
    public class MasterWarehouseService : IMasterWarehouseService
    {
        private readonly FPSDbContext _fpsContext;
        private readonly RFIDDbContext _shopifyContext;
        private readonly ILogger<MasterWarehouseService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MasterWarehouseService(FPSDbContext fpsContext, RFIDDbContext shopifyContext,ILogger<MasterWarehouseService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _fpsContext = fpsContext;
            _shopifyContext = shopifyContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

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

                if (req == null)
                {
                    return ResponseFactory<object>.Failed("Request is null");
                }

                var newData = new WarehouseReceive
                {
                    ReceiveNo = req.receiveNo,
                    CompanyCode = req.companyCode,
                    Warehouse = req.warehouse,
                    ReceiveDate = req.receiveDate?.Date,
                    ReceiveType = req.receiveType,
                    DeliveryNo = req.deliveryNo,
                    InvoiceNo = req.invoiceNo,
                    InvoiceDate = req.invoiceDate?.Date,
                    InputBy = req.createdBy,
                    InputDate = DateTime.Now,
                    EditBy = req.createdBy,
                    EditDate = DateTime.Now,
                    Remark = req.remark,
                };

                var res = await _fpsContext.warehouseReceives.AddAsync(newData);

                List<FPSWarehouseTransection> tranList = new List<FPSWarehouseTransection>();
                foreach (var item in req.rfidlist)
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
                    if (newData.ReceiveType == "Purchase")
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
                        Location = req.location,
                        RFId = item.rfid,
                        CompanyCode = newData.CompanyCode,
                        PONo = item.poNo,
                        POItemNo = item.poNoItem,
                        UOM = item.uom,
                        InType = newData.ReceiveType,
                        ReceiveDate = newData.ReceiveDate?.Date,
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

                if (existData == null)
                {
                    return ResponseFactory<object>.Failed("Not found receive No");
                }

                existData.ReceiveDate = req.receiveDate?.Date;
                existData.DeliveryNo = req.deliveryNo;
                existData.InvoiceNo = req.invoiceNo ?? null;
                existData.InvoiceDate = req.invoiceDate?.Date ?? null;
                existData.Remark = req.remark ?? null;
                existData.Warehouse = req.warehouse;
                existData.EditBy = req.createdBy ?? "system";
                existData.EditDate = DateTime.Now;

                foreach (var item in req.rfidlist)
                {
                    var exist = await _fpsContext.warehouseTransections.FirstOrDefaultAsync(t => t.RFId == item.rfid && t.ReceiveNo == existData.ReceiveNo);
                    if (exist != null)
                    {
                        exist.Warehouse = existData.Warehouse;
                        exist.Location = req.location;
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

                if (existRec == null)
                {
                    return ResponseFactory<object>.Failed("Not found receive packing list data");
                }
                var hasOutRfid = await _fpsContext.warehouseTransections
                    .AnyAsync(t => t.ReceiveNo == receiveNo && t.OutStatus);
                if (hasOutRfid)
                {
                    return ResponseFactory<object>.Failed("Have rfid is out from stock");
                }
                _fpsContext.RemoveRange(existRec);
                _fpsContext.RemoveRange(existTran);

                if (existRec.ReceiveType == "Purchase")
                {
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
            catch (Exception ex)
            {
                return ResponseFactory<List<WarehouseReceiveInDTO>>.Failed(ex.Message);
            }
        }

        // OutStock
        
        public async Task<ResponseDTO<List<WarehouseRequestOutMergeDTO>>> GetWarehouseRequestOut()
        {
            try
            {
                var result = await (
                    from m in _fpsContext.warehouseRequestOutMains
                    join d in _fpsContext.warehouseRequestOutDetails
                        on m.OutNo equals d.OutNo into md
                    from d in md.DefaultIfEmpty()   // ✅ LEFT JOIN
                    select new WarehouseRequestOutMergeDTO
                    {
                        // ===== Main =====
                        OutNo = m.OutNo,
                        RequestDate = m.RequestDate,
                        RequestBy = m.RequestBy,
                        OutType = m.OutType,
                        // ===== Detail =====
                        ItemCode = d != null ? d.ItemCode : null,
                        ColorCode = d != null ? d.ColorCode : null,
                        Size = d != null ? d.Size : null,
                        OutQty = d != null ? d.OutQty : null,
                        UOM = d != null ? d.UOM : null,

                    }
                ).ToListAsync();
                return ResponseFactory<List<WarehouseRequestOutMergeDTO>>.Ok("Success", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<WarehouseRequestOutMergeDTO>>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<WarehouseRequestOutMainDetailsDTO>>> GetWarehouseRequestOutByOutNo(string OutNo)
        {
            try
            {
                var result = await (
                    from m in _fpsContext.warehouseRequestOutMains
                    join d in _fpsContext.warehouseRequestOutDetails
                        on m.OutNo equals d.OutNo into md
                    from d in md.DefaultIfEmpty()   // ✅ LEFT JOIN
                    where m.OutNo == OutNo
                    select new WarehouseRequestOutMainDetailsDTO
                    {
                        // ===== Main =====
                        OutNo = m.OutNo,
                        RequestDate = m.RequestDate,
                        RequestBy = m.RequestBy,
                        OutType = m.OutType,
                        PoNo = m.PONo,
                        // ===== Detail =====
                        ItemCode = d != null ? d.ItemCode : null,
                        ColorCode = d != null ? d.ColorCode : null,
                        Size = d != null ? d.Size : null,
                        OutQty = d != null ? d.OutQty : null,
                        UOM = d != null ? d.UOM : null,
                        
                    }
                ).ToListAsync();
                return ResponseFactory<List<WarehouseRequestOutMainDetailsDTO>>.Ok("Success", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<WarehouseRequestOutMainDetailsDTO>>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<List<WarehouseRequestOutMergeDTO>>> GetListRequestOutstock()
        {
            try
            {

                var result = await _fpsContext
                    .Set<WarehouseRequestOutMergeDTO>()
                    .FromSqlRaw("EXEC sp_WarehouseRequestOut")
                    .AsNoTracking()
                    .ToListAsync();

                //var result = await (
                //    from m in _fpsContext.warehouseRequestOutMains
                //    join d in _fpsContext.warehouseRequestOutDetails
                //        on m.OutNo equals d.OutNo into md
                //    from d in md   // ✅ INNER JOIN
                //    where d.OutStatus == false
                //    select new WarehouseRequestOutMergeDTO
                //    {
                //        // ===== Main =====
                //        OutNo = m.OutNo,
                //        RequestDate = m.RequestDate,
                //        RequestBy = m.RequestBy,
                //        OutType = m.OutType,

                //        // ===== Detail =====
                //        ItemCode = d != null ? d.ItemCode : null,
                //        ColorCode = d != null ? d.ColorCode : null,
                //        Size = d != null ? d.Size : null,
                //        OutQty = d != null ? d.OutQty : null,
                //        UOM = d != null ? d.UOM : null,
                //    }
                //).ToListAsync();
                return ResponseFactory<List<WarehouseRequestOutMergeDTO>>.Ok("Success", result);
            }
            catch (Exception ex)
            {
                return ResponseFactory<List<WarehouseRequestOutMergeDTO>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> AutoRunOutNo(string company)
        {
            try
            {
                var result = _fpsContext.Set<GenReceiveNoResultDTO>()
                    .FromSqlRaw("EXEC sp_GenInventoryDocNo {0}, {1}", company, "RQO")
                    .AsEnumerable()  // ← บังคับให้ผลลัพธ์มาอยู่ใน memory ก่อน
                    .FirstOrDefault();


                return ResponseFactory<string>.Ok("Success", result.DocNo);
            }
            catch (Exception ex)
            {
                return ResponseFactory<string>.Failed(ex.Message);
            }
        }
        public async Task<ResponseDTO<object>> CreateRequestOutstock(WarehouseOutstockDTO req)
        {
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {
                if(req.Header == null)
                {
                    return ResponseFactory<object>.Failed("Request Main is null");
                }

                if(req.Items.Count == 0 || req.Items== null)
                {
                    return ResponseFactory<object>.Failed("Request Detail is null");
                }

                var newMain = new WarehouseRequestOutMain
                {
                    OutNo = req.Header.RequestNo,
                    RequestDate = req.Header.RequestDate.Date,
                    RequestBy = req.Header.RequestBy,
                    OutType = req.Header.OutType,
                    PONo = req.Header.PONo,
                    CreateBy = req.Header.RequestBy,
                    CreateDate = DateTime.Now,
                    EditBy = req.Header.CreateBy ?? "System",
                    EditDate = DateTime.Now,
                };

                List<WarehouseRequestOutDetail> detailList = new List<WarehouseRequestOutDetail>();
                foreach (var item in req.Items)
                {
                    var newDetail = new WarehouseRequestOutDetail
                    {
                        OutNo = req.Header.RequestNo,
                        ItemCode = item.ProductCode,
                        ColorCode = item.Color,
                        Size = item.Size,
                        OutQty = item.Qty,
                        UOM = item.Uom,
                        OutStatus = false,
                    };

                    detailList.Add(newDetail);
                }

                await _fpsContext.warehouseRequestOutMains.AddAsync(newMain);
                await _fpsContext.warehouseRequestOutDetails.AddRangeAsync(detailList);
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
        public async Task<ResponseDTO<object>> UpdateRequestOutstock(string requestOutNo, WarehouseOutstockDTO req)
        {
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {
                var existData = await _fpsContext.warehouseRequestOutMains.FirstOrDefaultAsync(t => t.OutNo == requestOutNo);
                if (existData == null)
                {
                    return ResponseFactory<object>.Failed("Not found request outstock No");
                }

                existData.RequestDate = req.Header.RequestDate;
                existData.RequestBy = req.Header.RequestBy;
                existData.PONo = req.Header.PONo;
                existData.OutType = req.Header.OutType;
                existData.EditBy = req.Header.CreateBy ?? "System";
                existData.EditDate = DateTime.Now;

                var existDetails = await _fpsContext.warehouseRequestOutDetails
                    .Where(t => t.OutNo == requestOutNo)
                    .ToListAsync();

                _fpsContext.warehouseRequestOutDetails.RemoveRange(existDetails);

                List<WarehouseRequestOutDetail> detailList = new List<WarehouseRequestOutDetail>();
                foreach (var item in req.Items) {
                    var newDetail = new WarehouseRequestOutDetail
                    {
                        OutNo = requestOutNo,
                        ItemCode = item.ProductCode,
                        ColorCode = item.Color,
                        Size = item.Size,
                        OutQty = item.Qty,
                        UOM = item.Uom,
                        OutStatus = false,
                    };

                    detailList.Add(newDetail);
                }

                await _fpsContext.warehouseRequestOutDetails.AddRangeAsync(detailList);
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
        public async Task<ResponseDTO<object>> DeleteRequestOutstock(string requestOutNo)
        {
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {
                var existMain = await _fpsContext.warehouseRequestOutMains.Where(t => t.OutNo.Equals(requestOutNo)).FirstOrDefaultAsync();
                var existDetail = await _fpsContext.warehouseRequestOutDetails.Where(t => t.OutNo.Equals(requestOutNo)).ToListAsync();

                if (existMain == null)
                {
                    return ResponseFactory<object>.Failed("Not found request outstock data");
                }
                _fpsContext.RemoveRange(existDetail);
                _fpsContext.RemoveRange(existMain);

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

        public async Task<ResponseDTO<List<WarehouseRequestOutDetail>>> GetDetailRequest(string requestOutNo)
        {
            try
            {
                var res = await _fpsContext.warehouseRequestOutDetails.Where(t => t.OutNo == requestOutNo).ToListAsync();

                return ResponseFactory<List<WarehouseRequestOutDetail>>.Ok("Success", res);
            }
            catch(Exception ex)
            {
                return ResponseFactory<List<WarehouseRequestOutDetail>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<object>> CreateWarehouseOutstock(CreateWarehouseOutDTO req)
        {
            if (req == null)
            {
                return ResponseFactory<object>.Failed("Request is null");
            }
            using var transaction = await _fpsContext.Database.BeginTransactionAsync();
            try
            {
                await _fpsContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_WarehouseUpdateOutStatus {0}, {1}, {2}, {3}",
                    req.outNo, req.productCode, req.colorCode, req.size
                );

                var userName = _httpContextAccessor.HttpContext?
                        .User?
                        .FindFirst(ClaimTypes.Name)?.Value;

                _logger.LogInformation(userName);

                foreach (var item in req.rfidlist)
                {
                    var tran = await _fpsContext.warehouseTransections
                        .FirstOrDefaultAsync(t => t.RFId == item && t.OutStatus == false);

                    if (tran == null)
                        return ResponseFactory<object>.Failed($"RFID {item} invalid");

                    tran.OutNo = req.outNo;
                    tran.OutStatus = true;
                    tran.OutType = req.outType;
                    tran.OutDate = req.outDate ?? DateTime.Now;
                    tran.InputBy = userName;
                    tran.InputOutDate = DateTime.Now;
                }

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

        public async Task<ResponseDTO<List<WarehouseShowRequestOutResonseDTO>>> GetShowRequestOUT(ShowRequestOutResponseDTO req)
        {
            try
            {
                var companyCode = _httpContextAccessor.HttpContext?
                    .User?
                    .Claims
                    .FirstOrDefault(c => c.Type == "CompanyCode")
                    ?.Value;

                var result = _fpsContext.Set<WarehouseShowRequestOutResonseDTO>()
                    .FromSqlRaw("EXEC sp_WarehouseShowRequestOut {0}, {1}, {2}, {3}, {4}", companyCode, req.OutNo, req.ItemCode, req.Color, req.Size)
                    .AsEnumerable()  // ← บังคับให้ผลลัพธ์มาอยู่ใน memory ก่อน
                    .ToList();


                return ResponseFactory<List<WarehouseShowRequestOutResonseDTO>>.Ok("Success", result);
            }
            catch(Exception ex)
            {
                return ResponseFactory<List<WarehouseShowRequestOutResonseDTO>>.Failed(ex.Message);
            }
        }

        public async Task<ResponseDTO<List<WarehouseRequestOutMain>>> GetRequestMainByOutNo(string outNo)
        {
            try
            {
                var res = await _fpsContext.warehouseRequestOutMains.Where(t => t.OutNo == outNo).ToListAsync();

                return ResponseFactory<List<WarehouseRequestOutMain>>.Ok("Success", res);
            }
            catch(Exception ex)
            {
                return ResponseFactory<List<WarehouseRequestOutMain>>.Failed(ex.Message);
            }
        }
    }
}
