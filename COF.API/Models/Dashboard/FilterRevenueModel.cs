using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COF.API.Models.Dashboard
{
    public class FilterRevenueModel
    {
        public int? ShopId { get; set; }
        public FilterType Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public enum FilterType
    {
        InWeek = 1,
        InMonth = 2,
        InYear = 3,
        Customize = 4
    }
}