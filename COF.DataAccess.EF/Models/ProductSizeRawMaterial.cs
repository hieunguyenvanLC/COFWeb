using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class ProductSizeRawMaterial : BaseEntity
    {
        public int ProductSizeId { get; set; }
        public virtual ProductSize ProductSize { get; set; }
        public int RawMaterialId { get; set; }
        public virtual RawMaterial RawMaterial { get; set; }
        public decimal Amount { get; set; }
    }
}
