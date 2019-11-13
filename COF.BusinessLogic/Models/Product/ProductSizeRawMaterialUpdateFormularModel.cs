using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Product
{
    public class ProductSizeRawMaterialUpdateFormularModel
    {
        public int ProductSizeId { get; set; }
        public int RawMaterialId { get; set; }
        public decimal Amount { get; set; }
    }
}
