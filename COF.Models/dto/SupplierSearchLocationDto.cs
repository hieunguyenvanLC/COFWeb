using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class SupplierSearchLocationDto
    {
        public int SupplierId { get; set; } // SupplierId (Primary key)

        public string Name { get; set; } // Name (length: 50)

        public string Avatar { get; set; } // Avatar (length: 200)

        public decimal Rating { get; set; }

        public string SupplierStatus { get; set; } // SupplierStatusId

        public string CreatedDate { get; set; } // CreatedDate

        public string Email { get; set; }

        public int? MainBranchId { get; set; } // MainBranchId

        public string PhoneNumber { get; set; } // PhoneNumber (length: 11)

        public List<ServiceWithPromotionDto> Services { get; set; }

        public List<BranchDto> Branches { get; set; }
    }
    public class ServiceWithPromotionDto
    {
        public int ServiceId { get; set; } // ServiceId (Primary key)


        public int SupplierId { get; set; } // SupplierId

        public string SupplierName { get; set; }

        public string Name { get; set; } // Name (length: 50)


        public decimal? Price { get; set; } // Price

        public int ServiceTypeId { get; set; }

        public int? PromotionPercent { get; set; }

        public decimal? OriginalPrice { get; set; }

        public String OriginalPriceDisplay
        {
            get
            {
                return Test.FormatVietnameseCurrency(OriginalPrice);
            }

        }
        public String PriceDisplay
        {
            get
            {
                return Test.FormatVietnameseCurrency(Price);
            }

        }





    }
    public static class Test
    {
        public static string FormatVietnameseCurrency(decimal? money)
        {
            if (money != null)
            {
                return String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", money.Value);
            }
            return "";

        }
    }
}
