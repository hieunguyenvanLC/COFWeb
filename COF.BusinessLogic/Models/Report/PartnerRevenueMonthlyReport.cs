using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace COF.BusinessLogic.Models.Report
{
    public class ShopRevenueMonthlyReport
    {
        public string Shop { get; set; }
        public int TotalOrder { get; set; }
        public double TotalMoney { get; set; }
    }

    public class OrderQueryModel
    {
        public int Id { get; set; }
        public double FinalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int ShopId { get; set; }
    }

    public class ShopRevenueReportModel
    {
        public string Header { get; set; }
        public int TotalOrder { get; set; }
        public double TotalMoney { get; set; }
        public int TotalUnit { get; set; }
        public List<CategoryReportModel> Details { get; set; }

        public MonthlyRevenueFilterByCakeOrDrinkCategoryModel MonthlyRevenueDetail { get; set; }
        public bool IsFilterByYear { get; set; }
    }

    public class CategoryReportModel
    {
        public string Type { get; set; }
        public  int TypeId { get; set; }
        public int TotalUnit { get; set; }
        public decimal TotalMoney { get;set; }
    }

    public class FilterRevenueByCakeOrDrinkCategoryModel
    {
        public string Title { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal CakeRevenue { get; set; }
        public decimal DrinkRevenue { get; set; }
    }

    public class MonthlyRevenueFilterByCakeOrDrinkCategoryModel : FilterRevenueByCakeOrDrinkCategoryModel
    {
        public  int Year { get; set; }
        public  int Month { get; set; }
        public  List<DayRevenueFilterByCakeOrDrinkCategoryModel> DayRevenues { get; set; }

        public MonthlyRevenueFilterByCakeOrDrinkCategoryModel(int year, int month, List<DayRevenueFilterByCakeOrDrinkCategoryModel> dayRevenues)
        {
            Year = year;
            Month = month;
            DayRevenues = dayRevenues;
            this.TotalRevenue = dayRevenues.Sum(x => x.TotalRevenue);
            this.DrinkRevenue = dayRevenues.Sum(x => x.DrinkRevenue);
            this.CakeRevenue = dayRevenues.Sum(x => x.CakeRevenue);
        }

        public MonthlyRevenueFilterByCakeOrDrinkCategoryModel()
        {

        }
    }
    public class DayRevenueFilterByCakeOrDrinkCategoryModel : FilterRevenueByCakeOrDrinkCategoryModel
    {
        public int Day { get; set; }
    }
}
