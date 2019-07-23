using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class PromotionSearchDto
    {
        public int SupplierId { get; set; }
        public string PromotionName { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDte { get; set; }
        public int pageSize { get; set; } = 20;
        public int page { get; set; } = 1;
    }
}
