using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;
using System.Diagnostics;
using System.Globalization;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {
        #region Document
        public async Task<(string error, bool isSuccess)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, type, payType, descripion, submitDate, receivedOrPaid));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreatePayDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.PayDoc, paymentType, descripion, submitDate, false));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.PayDiscount, PaymentType.Other,$" تخفیف سند {t.Entity.Serial} ",submitDate,false)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateRecDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.RecDoc, paymentType, descripion, submitDate, true));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.RecDiscount, PaymentType.Other,$" تخفیف سند {t.Entity.Serial} ",submitDate,true)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdatePayOrRecDocument(Guid docId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {
            try
            {
                var doc = await Entities.Include(t => t.RelatedDocuments)
                     .FirstOrDefaultAsync(t => t.Id == docId);

                if (doc == null)
                    return new("سند مورد نظر یافت نشد!!!", false);

                doc.PayType = paymentType;
                doc.Price = price;
                doc.Description = descripion;
                doc.SubmitDate = submitDate;

                // به روز رسانی تخفیف
                if (doc.RelatedDocuments.Count > 0)
                {
                    if (discount == null || discount == 0)
                    {
                        Entities.Remove(doc.RelatedDocuments.First());
                    }
                    else
                    {
                        doc.RelatedDocuments.First().Price = discount.Value;
                    }
                }
                else
                {
                    if (discount != null && discount != 0)
                    {
                        doc.RelatedDocuments.Add(new(doc.CustomerId, discount.Value, DocumntType.PayDiscount,
                            PaymentType.Other, $" تخفیف سند {doc.Serial} ", submitDate, false));
                    }
                }

                Entities.Update(doc);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        public async Task<(bool isSuccess, string errore)> DeleteDocument(Guid parameter)
        {
            var doc = await Entities
                .Include(t => t.RelatedDocuments)
                .Include(t => t.SellRemittances)
                .Include(t => t.BuyRemittances)
                .FirstOrDefaultAsync(t => t.Id == parameter);

            if (doc == null) return new(false, "مورد مدنظر یافت نشد!!!");

            foreach (var s in doc.SellRemittances.ToList()) doc.RemoveSellRem(s);

            foreach (var b in doc.BuyRemittances.ToList()) doc.RemoveBuyRem(b);

            try
            {
                foreach (var d in doc.RelatedDocuments.ToList()) Entities.Remove(d);

                Entities.Remove(doc);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }

            return new(true, string.Empty);
        }
        #endregion

        #region Invoice(CRUD)
        public async Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<SellRemittance> list = remittances.Select(t => new SellRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, t.Description)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.SellInv, PaymentType.Other, descripion, submitDate, false)
                .AddSellRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.PayCom,
                        PaymentType.Other,$" پورسانت فاکتور  {t.Entity.Serial} ",submitDate,true,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                }
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateSellDocument(Guid docId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            var doc = await Entities.Include(t => t.RelatedDocuments)
                .Include(s => s.SellRemittances)
                .FirstOrDefaultAsync(t => t.Id == docId);

            if (doc == null)
                return new("سند مورد نظر یافت نشد!!!", false);

            doc.Price = price;
            doc.Description = descripion;
            doc.SubmitDate = submitDate;

            // به روز رسانی تک تک قلم های فاکتور
            foreach (var item in remittances)
            {
                if (item.RremId == Guid.Empty)
                {
                    doc.SellRemittances.Add(new SellRemittance(
                        item.MaterialId,
                        item.AmountOf,
                        item.Price,
                        item.TotalPrice,
                        submitDate,
                        item.Description));
                }
                else
                {
                    var rem = doc.SellRemittances.FirstOrDefault(t => t.Id == item.RremId);
                    if (rem == null)
                        continue;
                    rem.MaterialId = item.MaterialId;
                    rem.AmountOf = item.AmountOf;
                    rem.Price = item.Price;
                    rem.TotalPrice = item.TotalPrice;
                    rem.SubmitDate = submitDate;
                    rem.Description = item.Description;
                }
            }

            // به روز رسانی پورسانت فاکتور
            if (doc.RelatedDocuments.Count > 0)
            {
                if (commission != null && commission != 0)
                {
                    doc.RelatedDocuments.First().Price = (long)(price * (commission.Value / 100));
                }
                else
                {
                    Entities.Remove(doc.RelatedDocuments.First());
                }
            }
            else
            {
                if (commission != null && commission != 0)
                {
                    doc.RelatedDocuments.Add(new(doc.CustomerId, (long)(price * (commission.Value / 100)), DocumntType.PayCom,
                     PaymentType.Other, $" پورسانت فاکتور  {doc.Serial} ", submitDate, true, (byte)commission.Value));
                }
            }

            try
            {
                Entities.Update(doc);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateBuyDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<BuyRemittance> list = remittances.Select(t => new BuyRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.BuyInv, PaymentType.Other, descripion, submitDate, true)
                .AddBuyRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.RecCom,
                        PaymentType.Other,$" پورسانت فاکتور {t.Entity.Serial} ",submitDate,false,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateBuyDocument(Guid docId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            var doc = await Entities.Include(t => t.RelatedDocuments)
                .Include(s => s.BuyRemittances)
                .FirstOrDefaultAsync(t => t.Id == docId);

            if (doc == null)
                return new("سند مورد نظر یافت نشد!!!", false);

            doc.Price = price;
            doc.Description = descripion;
            doc.SubmitDate = submitDate;

            // به روز رسانی تک تک قلم های فاکتور
            foreach (var item in remittances)
            {
                if (item.RremId == Guid.Empty)
                {
                    doc.BuyRemittances.Add(new BuyRemittance(
                        item.MaterialId,
                        item.AmountOf,
                        item.Price,
                        item.TotalPrice,
                        submitDate,
                        item.Description));
                }
                else
                {
                    var rem = doc.BuyRemittances.FirstOrDefault(t => t.Id == item.RremId);
                    if (rem == null)
                        continue;
                    rem.MaterialId = item.MaterialId;
                    rem.AmountOf = item.AmountOf;
                    rem.Price = item.Price;
                    rem.TotalPrice = item.TotalPrice;
                    rem.SubmitDate = submitDate;
                    rem.Description = item.Description;
                }
            }

            // به روز رسانی پورسانت فاکتور
            if (doc.RelatedDocuments.Count > 0)
            {
                if (commission != null && commission != 0)
                {
                    doc.RelatedDocuments.First().Price = (long)(price * (commission.Value / 100));
                }
                else
                {
                    Entities.Remove(doc.RelatedDocuments.First());
                }
            }
            else
            {
                if (commission != null && commission != 0)
                {
                    doc.RelatedDocuments.Add(new(doc.CustomerId, (long)(price * (commission.Value / 100)), DocumntType.PayCom,
                     PaymentType.Other, $" پورسانت فاکتور  {doc.Serial} ", submitDate, true, (byte)commission.Value));
                }
            }

            try
            {
                Entities.Update(doc);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t => t.CreationTime).Where(t => t.Type == type).Select(c => c.Serial).FirstOrDefaultAsync()).ToString();
        }

        public async Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetSellInvoiceDetail(Guid invoiceId)
        {
            var inv = await TableNoTracking
                .Include(r => r.SellRemittances)
                .Include(r => r.RelatedDocuments)
                .Where(t => t.Id == invoiceId)
                .Select(c => new InvoiceDetailUpdateDto()
                {
                    CustomerId = c.CustomerId,
                    Serial = c.Serial.ToString(),
                    Date = c.SubmitDate,
                    TotalPrice = c.Price,
                    Commission = c.RelatedDocuments.Sum(t => t.Commission),
                    CommissionPrice = c.RelatedDocuments.Sum(t => t.Price),
                    InvoiceDescription = c.Description,
                    RemList = c.SellRemittances.Select(t => new RemittanceListViewModel()
                    {
                        AmountOf = t.AmountOf,
                        Description = t.Description,
                        MaterialId = t.MaterialId,
                        Price = t.Price,
                        RremId = t.Id,
                        TotalPrice = t.TotalPrice
                    }).ToList(),
                }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new InvoiceDetailUpdateDto());
            }

            return new(true, inv);
        }

        public async Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetBuyInvoiceDetail(Guid invoiceId)
        {
            var inv = await TableNoTracking
                .Include(r => r.BuyRemittances)
                .Include(r => r.RelatedDocuments)
                .Where(t => t.Id == invoiceId)
                .Select(c => new InvoiceDetailUpdateDto()
                {
                    CustomerId = c.CustomerId,
                    Serial = c.Serial.ToString(),
                    Date = c.SubmitDate,
                    TotalPrice = c.Price,
                    Commission = c.RelatedDocuments.Sum(t => t.Commission),
                    CommissionPrice = c.RelatedDocuments.Sum(t => t.Price),
                    InvoiceDescription = c.Description,
                    RemList = c.BuyRemittances.Select(t => new RemittanceListViewModel()
                    {
                        AmountOf = t.AmountOf,
                        Description = t.Description,
                        MaterialId = t.MaterialId,
                        Price = t.Price,
                        RremId = t.Id,
                        TotalPrice = t.TotalPrice
                    }).ToList(),
                }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new InvoiceDetailUpdateDto());
            }

            return new(true, inv);
        }

        public async Task<(bool isSuccess, DocUpdateDto itm)> GetDocumentById(Guid docId)
        {
            var inv = await TableNoTracking
                 .Where(t => t.Id == docId)
                 .Include(r => r.RelatedDocuments)
                 .Select(c => new DocUpdateDto()
                 {
                     CustomerId = c.CustomerId,
                     Serial = c.Serial.ToString(),
                     Date = c.SubmitDate,
                     Type = c.PayType,
                     DocDescription = c.Description,
                     Price = c.Price,
                     Dicount = c.RelatedDocuments.Sum(t => t.Price)
                 }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new DocUpdateDto());
            }

            return new(true, inv);
        }

        #endregion

        #region Status
        public async Task<long> GetDebt(Guid customerId)
        {
            return await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

        }

        public async Task<long> GetCredit(Guid customerId)
        {
            return await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();
        }

        public async Task<UserDebtStatus> GetStatus(Guid customerId)
        {
            var tal = await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            var bed = await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            long res = tal - bed;
            if (res > 0)
            {
                return new UserDebtStatus
                {
                    Status = "بدهکار",
                    Amount = res,
                    Credit = "0",
                    Debt = Math.Abs(res).ToString("N0"),

                };
            }
            if (res < 0)
            {
                return new UserDebtStatus()
                {
                    Status = "طلبکار",
                    Amount = res,
                    Debt = "0",
                    Credit = Math.Abs(res).ToString("N0")
                };
            }
            return new UserDebtStatus()
            {
                Status = "تسویه",
                Amount = 0,
                Credit = "0",
                Debt = "0"
            };
        }
        #endregion

        #region report
        public async Task<PagedResulViewModel<InvoiceListDtos>> GetInvoicesByDate(DateTime startTime,
            DateTime endTime,
            string desc,
            Guid cusId,
            bool leftOver,
            bool ignorePagination,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            int i = 1;
            PersianCalendar pc = new();
            List<InvoiceListDtos> Remittances = [];
            var MyDoc = await TableNoTracking
                .Where(st => leftOver || st.SubmitDate >= startTime)
                .Where(et => et.SubmitDate < endTime)
                .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                .Where(p => p.CustomerId == cusId)
                .Select(t => new InvoiceDto()
                {
                    Id = t.Id,
                    Type = t.Type,
                    Date = t.SubmitDate,
                    Serial = t.Serial,
                    Description = t.Description,
                    Price = t.Price,
                    ReceivedOrPaid = t.IsReceived
                }).OrderBy(p => p.Date)
                .ToListAsync();


            if (leftOver)
            {
                InvoiceListDtos rem = new()
                {
                    Row = 0,
                    Date = startTime,
                    ShamsiDate = startTime.ToShamsiDate(pc),
                    Description = "باقی مانده از قبل",
                    Bed = MyDoc.Where(p => p.Date < startTime && !p.ReceivedOrPaid).Sum(p => p.Price),
                    Bes = MyDoc.Where(p => p.Date < startTime && p.ReceivedOrPaid).Sum(p => p.Price),
                    Type = DocumntType.Other
                };
                Remittances.Add(rem);
            }

            Remittances.AddRange(MyDoc.Where(p => !p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDtos
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Serial = t.Serial.ToString(),
                Id = t.Id,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = t.Price,
                Bes = 0,
            }).ToList());

            Remittances.AddRange(MyDoc.Where(p => p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDtos
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Id = t.Id,
                Serial = t.Serial.ToString(),
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = 0,
                Bes = t.Price,
            }).ToList());

            Remittances = [.. Remittances.OrderBy(t => t.Date)];

            foreach (var item in Remittances)
            {
                item.Row = i;
                long bed = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bed);
                long bes = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bes);
                if (item.Type == DocumntType.PayDoc || item.Type == DocumntType.RecDoc || item.Type == DocumntType.SellInv || item.Type == DocumntType.BuyInv)
                {
                    item.IsDeletable = true;
                    item.IsEditable = true;
                }

                item.LeftOver = Math.Abs(bed - bes);
                if (bes > bed)
                {
                    item.Status = "طلبکار";
                }
                else if (bed > bes)
                {
                    item.Status = "بدهکار";
                }
                else
                {
                    item.Status = "تسویه";
                }
                i++;
            }

            var totalCount = Remittances.Count;

            if (!ignorePagination)
            {
                if (isInit)
                {
                    pageNum = totalCount / pageCount;
                    if (totalCount % pageCount != 0)
                    {
                        pageNum++;
                    }
                }
                Remittances = Remittances.Skip((pageNum - 1) * pageCount).Take(pageCount).ToList();
            }

            return new PagedResulViewModel<InvoiceListDtos>(totalCount, pageCount, pageNum, Remittances);
        }

        public async Task<PagedResulViewModel<DetailRemittanceDto>> GetRemittancesByDate(DateTime startTime,
            DateTime endTime,
            Guid cusId,
            bool leftOver,
            string desc,
            bool ignorePagination,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {

            int i = 1;
            List<DetailRemittanceDto> Remittances = [];

            // گرفتن تمام سند های مربوط از دیتابیس
            Remittances.AddRange(await TableNoTracking
                .Where(st => st.SubmitDate >= startTime)
                .Where(et => et.SubmitDate < endTime)
                .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                .Where(p => p.CustomerId == cusId)
                .Where(s => s.Type != DocumntType.SellInv && s.Type != DocumntType.BuyInv)
                .Select(t => new DetailRemittanceDto()
                {
                    Serial = t.Serial.ToString(),
                    Date = t.SubmitDate,
                    Bes = t.Price,
                    Bed = t.Price,
                    IsRecived = t.IsReceived,
                    MaterialName = t.Description
                }).ToListAsync());


            // اضافه کردن فاکتور های فروش
            Remittances.AddRange(await (from doc in DbContext.Set<Document>()
                      .AsNoTracking()
                      .Where(st => st.SubmitDate >= startTime)
                      .Where(et => et.SubmitDate < endTime)
                      .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                      .Where(p => p.CustomerId == cusId)

                                        join sellRem in DbContext.Set<SellRemittance>()
                                                                on doc.Id equals sellRem.DocumentId

                                        orderby doc.CreationTime descending
                                        select new DetailRemittanceDto()
                                        {
                                            Date = doc.SubmitDate,
                                            IsRecived = doc.IsReceived,
                                            AmuontOf = sellRem.AmountOf.ToString(),
                                            Serial = doc.Serial.ToString(),
                                            Description = sellRem.Description,
                                            Price = sellRem.Price.ToString("N0"),
                                            Bed = sellRem.TotalPrice,
                                            Bes = 0,
                                            MaterialName = sellRem.Material.Name,
                                            Unit = sellRem.Material.Unit.Name,
                                        }).ToListAsync());

            // اضافه کردن فاکتورهای خرید
            Remittances.AddRange(await (from doc in DbContext.Set<Document>()
               .AsNoTracking()
               .Where(st => st.SubmitDate >= startTime)
               .Where(et => et.SubmitDate < endTime)
               .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
               .Where(p => p.CustomerId == cusId)

                                        join buyRem in DbContext.Set<BuyRemittance>()
                                                                on doc.Id equals buyRem.DocumentId

                                        orderby doc.CreationTime descending
                                        select new DetailRemittanceDto()
                                        {
                                            Date = doc.SubmitDate,
                                            IsRecived = doc.IsReceived,
                                            AmuontOf = buyRem.AmountOf.ToString(),
                                            Serial = doc.Serial.ToString(),
                                            Description = buyRem.Description,
                                            Bed = buyRem.TotalPrice,
                                            Price = buyRem.Price.ToString("N0"),
                                            Bes = 0,
                                            MaterialName = buyRem.Material.Name,
                                            Unit = buyRem.Material.Unit.Name,
                                        }).ToListAsync());

            PersianCalendar pc = new();

            // اگر تیک باقیمانده زده باشد
            if (leftOver)
            {
                DetailRemittanceDto rem = new()
                {
                    Row = 0,
                    Date = startTime,
                    ShamsiDate = startTime.ToShamsiDate(pc),
                    MaterialName = "باقی مانده از قبل",
                    Bed = await TableNoTracking.Where(p => p.SubmitDate < startTime && !p.IsReceived).SumAsync(p => p.Price),
                    Bes = await TableNoTracking.Where(p => p.SubmitDate < startTime && p.IsReceived).SumAsync(p => p.Price),
                };
                Remittances.Add(rem);
            }

            // به ترتیب کردن اقلام صورتحساب
            Remittances = [.. Remittances.OrderBy(t => t.Date)];

            // شماره گذاری و تعیین وضعیت
            foreach (var item in Remittances)
            {
                item.Row = i;
                item.ShamsiDate = item.Date.ToShamsiDate(pc);
                long bed = Remittances.Where(p => p.Row <= i && p.Row >= 1).Select(p => p.Bed).Sum();
                long bes = Remittances.Where(p => p.Row <= i && p.Row >= 1).Select(p => p.Bes).Sum();
                item.LeftOver = Math.Abs(bed - bes);
                if (item.IsRecived)
                {
                    item.Bed = 0;
                }
                else
                {
                    item.Bes = 0;
                }

                if (bes > bed)
                {
                    item.Status = "طلبکار";
                }
                else if (bed > bes)
                {
                    item.Status = "بدهکار";
                }
                else
                {
                    item.Status = "تسویه";
                }
                i++;
            }

            var totalCount = Remittances.Count;

            if (!ignorePagination)
            {
                if (isInit)
                {
                    pageNum = totalCount / pageCount;
                    if (totalCount % pageCount != 0)
                    {
                        pageNum++;
                    }
                }
                Remittances = Remittances.Skip((pageNum - 1) * pageCount).Take(pageCount).ToList();
            }

            return new PagedResulViewModel<DetailRemittanceDto>(totalCount, pageCount, pageNum, Remittances);
        }

        public async Task<PagedResulViewModel<MaterialReportDto>> GetMaterialReport(int id,
            bool isBuy,
            bool isSell,
            DateTime startDate,
            DateTime endDate,
            bool ignorePagination,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            int i = 1;
            List<MaterialReportDto> matList = [];

            if (isSell)
            {
                matList.AddRange(await (from doc in DbContext.Set<Document>()
                          .AsNoTracking()
                          .Where(st => st.SubmitDate >= startDate)
                          .Where(et => et.SubmitDate < endDate)

                                        join cus in DbContext.Set<Customer>()
                                                                on doc.CustomerId equals cus.Id

                                        join sellRem in DbContext.Set<SellRemittance>()
                                                                on doc.Id equals sellRem.DocumentId

                                        where sellRem.MaterialId == id
                                        orderby doc.CreationTime descending
                                        select new MaterialReportDto()
                                        {
                                            Date = doc.SubmitDate,
                                            Status = "فروش",
                                            AmuontOf = sellRem.AmountOf.ToString(),
                                            CusName = cus.Name,
                                            MatName = sellRem.Material.Name,
                                            Price = sellRem.Price.ToString("N0"),
                                        }).ToListAsync());
            }

            if (isBuy)
            {
                matList.AddRange(await (from doc in DbContext.Set<Document>()
                          .AsNoTracking()
                          .Where(st => st.SubmitDate >= startDate)
                          .Where(et => et.SubmitDate < endDate)

                                        join cus in DbContext.Set<Customer>()
                                                                on doc.CustomerId equals cus.Id

                                        join buyRem in DbContext.Set<BuyRemittance>()
                                                                on doc.Id equals buyRem.DocumentId

                                        where buyRem.MaterialId == id
                                        orderby doc.CreationTime descending
                                        select new MaterialReportDto()
                                        {
                                            Date = doc.SubmitDate,
                                            Status = "خرید",
                                            AmuontOf = buyRem.AmountOf.ToString(),
                                            CusName = cus.Name,
                                            MatName = buyRem.Material.Name,
                                            Price = buyRem.Price.ToString("N0"),
                                        }).ToListAsync());
            }

            matList = [.. matList.OrderBy(t => t.Date)];

            PersianCalendar pc = new();
            foreach (var item in matList)
            {
                item.Row = i++;
                item.ShamsiDate = item.Date.ToShamsiDate(pc);
            }
            var totalCount = matList.Count;

            if (!ignorePagination)
            {
                if (isInit)
                {
                    pageNum = totalCount / pageCount;
                    if (totalCount % pageCount != 0)
                    {
                        pageNum++;
                    }
                }
                matList = matList.Skip((pageNum - 1) * pageCount)
                    .Take(pageCount)
                    .ToList();
            }

            return new PagedResulViewModel<MaterialReportDto>(totalCount, pageCount, pageNum, matList);

        }

        public async Task<List<SummaryDoc>> GetSummaryDocs(Guid? CusId, DocumntType type)
        {
            PersianCalendar pc = new();
            var list = await (from doc in DbContext.Set<Document>()
                                               .AsNoTracking()
                                               .Where(t => t.Type == type)
                                               .Where(t => CusId == null || t.CustomerId == CusId)

                              join cus in DbContext.Set<Customer>()
                                                      on doc.CustomerId equals cus.Id

                              orderby doc.CreationTime descending
                              select new SummaryDoc()
                              {
                                  SubmitDate = doc.SubmitDate,
                                  Cus_Name = cus.Name,
                                  Price = doc.Price.ToString("N0"),
                              })
                      .Take(15)
                      .ToListAsync();

            list.ForEach(c =>
            {
                c.ShamsiDate = c.SubmitDate.ToShamsiDate(pc);
            });
            return list;
        }

        /// <summary>
        /// دریافت لیست بدهکاران
        /// </summary>
        public async Task<CreditorsOrDebtorsReport> GetDebtorsReport(DateTime startDate, DateTime endDate)
        {

          //  var t = await (from doc in DbContext.Set<Document>()
          //.AsNoTracking()
          //.Where(st => st.SubmitDate >= startDate)
          //.Where(et => et.SubmitDate < endDate)

          //                 group doc by doc.CustomerId into cusId

          //                 let join cus in DbContext.Set<Customer>()
          //                               on cusId equals cus.Id
          
          //                 select new CreditorsOrDebtorsReport()
          //                 {
          //                     CusName = cus.Name

          //                 }).ToListAsync();

            return null;
        }

        /// <summary>
        /// دریافت لیست طلبکاران
        /// </summary>
        /// <returns></returns>
        public async Task<CreditorsOrDebtorsReport> GetcreditorsReport(DateTime startDate, DateTime endDate)
        {
            return null;
        }



        public async Task<PagedResulViewModel<DalyBookDto>> GetDalyBook(int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            var list = await (from doc in DbContext.Set<Document>()
                                               .AsNoTracking()
                                               .Where(t => t.CreationTime.Day == DateTime.Now.Day)
                              join cus in DbContext.Set<Customer>()
                                                                       on doc.CustomerId equals cus.Id

                              orderby doc.CreationTime descending
                              select new DalyBookDto()
                              {
                                  SubmitDate = doc.SubmitDate,
                                  Bed = doc.IsReceived ? "0" : doc.Price.ToString("N0"),
                                  Bes = doc.IsReceived ? doc.Price.ToString("N0") : "0",
                                  CustomerName = cus.Name,
                                  Description = doc.Description,
                                  Id = doc.Id,
                                  Type = doc.Type,
                                  Serial = doc.Serial.ToString()
                              })
                      .ToListAsync();

            int row = 1;
            PersianCalendar pc = new();
            foreach (var item in list)
            {
                item.ShamsiDate = item.SubmitDate.ToShamsiDate(pc);
                item.Row = row;
                row++;
            }
            var totalCount = list.Count;
            list = list.Skip(--pageNum * pageCount).Take(pageCount).ToList();
            return new PagedResulViewModel<DalyBookDto>(totalCount, pageCount, pageNum, list);
        }
        #endregion
    }
}
