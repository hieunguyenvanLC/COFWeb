using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class PromotionDetailDto
    {
        public int PromotionDetailId { get; set; } // PromotionDetailId (Primary key)


        public int? PromotionPercent { get; set; } // PromotionPercent


        public int? PromotionStatusId { get; set; } // PromotionStatusId

        public int? ServiceId { get; set; } // ServiceId

        public string ServiceName { get; set; }

        public decimal? OriginalPrice { get; set; } // OriginalPrice

        public int? PromotionId { get; set; } // PromotionId

        public string PromotionPriceDisplay { get; set; }
        public string OriginalPricePriceDisplay { get; set; }
    }
}
