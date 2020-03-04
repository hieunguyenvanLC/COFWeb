using System;
using System.Collections.Generic;
using System.Globalization;
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
        public DateTime? _fromDate
        {
            get
            {              
                if (!string.IsNullOrEmpty(FromDate))
                {
                    return DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                return null;
            }
        }
        public DateTime? _toDate
        {
            get
            {
                if (!string.IsNullOrEmpty(ToDate))
                {
                    return DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                return null;
            }
        }

        public int Year { get; set; }
    }
    public enum FilterType
    {
        InWeek = 1,
        InMonth = 2,
        InYear = 3,
        Customize = 4,
        Years = 5
    }
}