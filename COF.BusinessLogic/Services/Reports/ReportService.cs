﻿using COF.BusinessLogic.Models.Report;
using COF.BusinessLogic.Services.AzureBlob;
using COF.BusinessLogic.Services.Export;
using COF.Common.Helper;
using COF.DataAccess.EF.Infrastructure;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Reports
{
    public interface IReportService
    {
        byte[] ExportDailyOrderReport(string fileName);
        List<ShopRevenueMonthlyReport> GetPartnerRevenueMonthlyReport(int partnerId);
        List<ShopRevenueReportModel> GetShopRevenueReportImMonthModels(int partnerId,int? shopId);
        List<ShopRevenueReportModel> GetShopRevenueReportInYearModels(int partnerId, int? shopId);
        List<ShopRevenueReportModel> GetShopRevenueReportInRange(int partnerId, int? shopId, DateTime fromDate, DateTime toDate);

        List<ShopRevenueReportModel> GetShopRevenueReportInYearsModels(int partnerId, int? shopId, int year);

    }
    public class ReportService : IReportService
    {
        #region fields
        private readonly IPartnerService _partnerService;
        private readonly IExcelExportService _excelExportService;
        private readonly IOrderService _orderService;
        private readonly IAzureBlobSavingService _azureBlobSavingService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ctor
        public ReportService(
            IPartnerService partnerService,
            IExcelExportService excelExportService,
            IOrderService orderService,
            IAzureBlobSavingService azureBlobSavingService,
            IProductCategoryService productCategoryService,
            IUnitOfWork unitOfWork
            )
        {
            _partnerService = partnerService;
            _excelExportService = excelExportService;
            _orderService = orderService;
            _azureBlobSavingService = azureBlobSavingService;
            _productCategoryService = productCategoryService;
            _unitOfWork = unitOfWork;
        }
        #endregion
        public byte[] ExportDailyOrderReport(string fileName)
        {
            var partners = _partnerService.GetAll();
            foreach (var partner in partners.Result)
            {
                var result = GetShopRevenueReportImMonthModels(partner.Id, null);
                var category = _productCategoryService.GetAll();
                var exportData = new ExportDailyModel()
                {
                    Categories = category,
                    ToDayRevenue = result.LastOrDefault(),
                };
                
                var bytes = _excelExportService.ExportExcelOutsource(exportData);
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                _azureBlobSavingService.SavingFileToAzureBlob(bytes, fileName, contentType, AzureHelper.DailyOrderExportContainer);
                return bytes;
            }

            return null;
        }

        public List<ShopRevenueMonthlyReport> GetPartnerRevenueMonthlyReport(int partnerId)
        {
            var partner = _partnerService.GetById(partnerId);
            var shops = partner.Result.Shops.ToList();
            

            var sql = "select Id, FinalAmount, ShopId,OrderStatus from [Order] where PartnerId = @p0 and Year(CheckInDate) = @p1 and Month(CheckInDate) = @p2";
            var allOrders =  _unitOfWork.Context.Database.SqlQuery<OrderQueryModel>(sql, partnerId, DateTimeHelper.CurentVnTime.Year, DateTimeHelper.CurentVnTime.Month).ToList();
            allOrders = allOrders.Where(x => x.OrderStatus == OrderStatus.PosFinished).ToList();
            var result = shops.Select(shop => new ShopRevenueMonthlyReport
            {
                Shop = shop.ShopName,
                TotalMoney = allOrders
                                .Where(x => x.ShopId == shop.Id)
                                .Select(x => x.FinalAmount).DefaultIfEmpty(0).Sum(),
                TotalOrder = allOrders
                                .Count(x => x.ShopId == shop.Id)
            }).ToList();
            return result;

        }

        public List<ShopRevenueReportModel> GetShopRevenueReportImMonthModels(int partnerId, int? shopId)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInMonthByShopId(shopId.Value);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInMonth(partnerId);
                allOrders = queryRes.Result;
            }
            allOrders = allOrders.Where(x => x.OrderStatus == OrderStatus.PosFinished).ToList();
            var currentDate = DateTime.UtcNow.AddHours(7); 
            var dateInMonth = Enumerable.Range(1, DateTime.DaysInMonth(currentDate.Year,currentDate.Month))  // Days: 1, 2 ... 31 etc.
                    .Select(day => new DateTime(currentDate.Year, currentDate.Month, day)) // Map each day to a date
                    .Where(day => day.Date <= currentDate.Date)
                    .ToList(); // Load dates into a list
            var result = dateInMonth.Select(x => {

                var tmp = new ShopRevenueReportModel
                {
                    Header = x.Date.ToString("dd/MM"),
                    Details = GetOrderDetails(allOrders.Where(y => y.CheckInDate.Date == x.Date).ToList()),
                    TotalMoney = allOrders.Where(y => y.CheckInDate.Date == x.Date)
                                .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum()
                                ,
                    TotalOrder = allOrders.Where(y => y.CheckInDate.Date == x.Date).Count(),
                };
                tmp.TotalUnit = tmp.Details.Sum(y => y.TotalUnit);
                return tmp;
            }).ToList();

            return result;
        }

        public List<ShopRevenueReportModel> GetShopRevenueReportInRange(int partnerId, int? shopId, DateTime fromDate, DateTime toDate)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInRangeShopId(shopId.Value,fromDate,toDate);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInRange(partnerId,fromDate,toDate);
                allOrders = queryRes.Result;
            }
            allOrders = allOrders.Where(x => x.OrderStatus == OrderStatus.PosFinished).ToList();

            var groupByData = allOrders.GroupBy(x => x.CheckInDate.Date).OrderBy(x => x.Key).ToList();

            var result = new List<ShopRevenueReportModel>();
            foreach (var item in groupByData)
            {
                var orders = item.ToList();
                var tmp = new ShopRevenueReportModel
                {
                    Header = $"{item.Key.ToString("dd/MM/yy")}",
                    Details = GetOrderDetails(orders),
                    TotalMoney = orders
                                 .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum(),
                    TotalOrder = orders.Count()
                };
                tmp.TotalUnit = tmp.Details.Sum(x => x.TotalUnit);
                result.Add(tmp);
            }
            return result;

        }

        public List<ShopRevenueReportModel> GetShopRevenueReportInYearsModels(int partnerId, int? shopId, int year)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInYearsByShopId(shopId.Value, year);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInYears(partnerId, year);
                allOrders = queryRes.Result;
            }

            var months = allOrders.Select(x => x.CheckInDate.Month).Distinct().ToList();

            var result = new List<ShopRevenueReportModel>();
            var allCategories = _productCategoryService.GetAll();
            for (int i = 1; i <= 12; i++)
            {
                var tmp = new ShopRevenueReportModel
                {
                    Header = $"Tháng {i} - {year} ",
                };
                if (months.Contains(i))
                {
                    var data = allOrders.Where(y => y.CheckInDate.Month == i).ToList();
                    tmp.Details = GetOrderDetails(data);
                    tmp.TotalMoney = data
                               .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum();
                    tmp.TotalOrder = data.Count();

                    tmp.TotalUnit = tmp.Details.Sum(x => x.TotalUnit);

                    tmp.MonthlyRevenueDetail = new MonthlyRevenueFilterByCakeOrDrinkCategoryModel(year, i,
                        FilterByCakeOrDrinkCategoryByDay(year, i, allCategories, data));
                }
                else
                {
                    tmp.MonthlyRevenueDetail = new MonthlyRevenueFilterByCakeOrDrinkCategoryModel(year, i,
                        new List<DayRevenueFilterByCakeOrDrinkCategoryModel>());
                }
                tmp.IsFilterByYear = true;
                result.Add(tmp);
            }
            return result;
        }

        public List<ShopRevenueReportModel> GetShopRevenueReportInYearModels(int partnerId, int? shopId)
        {
            var partner = _partnerService.GetById(partnerId);
            List<Order> allOrders = null;
            if (shopId != null)
            {
                var queryRes = _orderService.GetOrdersInYearByShopId(shopId.Value);
                allOrders = queryRes.Result;
            }
            else
            {
                var queryRes = _orderService.GetOrdersInYear(partnerId);
                allOrders = queryRes.Result;
            }
            allOrders = allOrders.Where(x => x.OrderStatus == OrderStatus.PosFinished).ToList();
            var currentDate = DateTime.UtcNow.AddHours(7);
            var result = new  List<ShopRevenueReportModel>();
            for (int i = 1; i <= currentDate.Month; i++)
            {
                var tmp = new ShopRevenueReportModel
                {
                    Header = $"Tháng {i}",
                    Details = GetOrderDetails(allOrders.Where(y => y.CheckInDate.Month == i).ToList()),
                    TotalMoney = allOrders.Where(x => x.CheckInDate.Month == i)
                                 .Select(y => y.FinalAmount).DefaultIfEmpty(0).Sum(),
                    TotalOrder = allOrders.Where(x => x.CheckInDate.Month == i).Count()
                };
                tmp.TotalUnit = tmp.Details.Sum(x => x.TotalUnit);
                result.Add(tmp);
            }
            return result;
        }

        private List<CategoryReportModel> GetOrderDetails(List<Order> orders)
        {
            var allCategories = _productCategoryService.GetAll();
            var orderDetails = orders.SelectMany(x => x.OrderDetails).ToList();
            var groupBy = orderDetails.GroupBy(x => x.CategoryId).ToList();
            var result = groupBy.Select(x => new CategoryReportModel
            {
                Type = allCategories.FirstOrDefault(y => y.Id == x.Key).Name,
                TypeId =  x.Key,
                TotalUnit = x.Sum(y => y.Quantity),
                TotalMoney = x.Sum(y => 1.0m *  y.Quantity * y.UnitPrice)
            }).ToList();
            return result;
        }

        //private List<CategoryReportModel> GetOrderDetails(List<OrderModel> orders)
        //{
        //    var allCategories = _productCategoryService.GetAll();
        //    var orderDetails = orders.SelectMany(x => x.OrderDetails).ToList();
        //    var groupBy = orderDetails.GroupBy(x => x.CategoryId).ToList();
        //    var result = groupBy.Select(x => new CategoryReportModel
        //    {
        //        Type = allCategories.FirstOrDefault(y => y.Id == x.Key).Name,
        //        TotalUnit = x.Sum(y => y.Quantity),
        //        TotalMoney = x.Sum(y => 1.0m * y.Quantity * y.UnitPrice)
        //    }).ToList();
        //    return result;
        //}

        private List<DayRevenueFilterByCakeOrDrinkCategoryModel> FilterByCakeOrDrinkCategoryByDay(int year, int month, List<Category> categories, List<Order> orders)
        {
            var catogoryNames = new String[]
            {
                "Bánh",
                "Món thêm"
            };
            var cakeIds = categories.Where(x => catogoryNames.Contains(x.Name)).Select(x => x.Id).ToList();
            var dayRevenues = new List<DayRevenueFilterByCakeOrDrinkCategoryModel>();

            var currentDate = DateTime.UtcNow.AddHours(7);
            var dateInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)) 
                .Select(day => new DateTime(year, month, day)) 
                .ToList(); // Load dates into a list


            var result = dateInMonth.Select(x =>
            {

                var dayOrders = orders.Where(p => p.CheckInDate.Date == x.Date).ToList();
                var orderDetails = dayOrders.SelectMany(p => p.OrderDetails).ToList();
                var cakeRevenue = orderDetails.Where(p => cakeIds.Contains(p.CategoryId)).Sum(p => 1.0m * p.Quantity * p.UnitPrice);
                var total = (decimal) dayOrders.Sum(p => p.FinalAmount);
                var drinkRevenue = (decimal) dayOrders.Sum(p => p.FinalAmount) - cakeRevenue;
                var tmp = new DayRevenueFilterByCakeOrDrinkCategoryModel
                {
                    Title = $"{x.Day}",
                    Day =  x.Day,
                    CakeRevenue = cakeRevenue,
                    DrinkRevenue = drinkRevenue,
                    TotalRevenue = total,
                };
                return tmp;
            }).ToList();
            return result;
        }

    }
}
