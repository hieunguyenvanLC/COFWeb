using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class PromotionCreateDto
    {
        public int PromotionId { get; set; }
        public string PromotionTile { get; set; }
        public string Description { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public List<PromotionDetailDto> PromotionDetailDto { get; set; }
        public int SupplierId { get; set; }
    }
}
