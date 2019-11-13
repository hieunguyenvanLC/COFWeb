using COF.BusinessLogic.Models.Product;
using COF.BusinessLogic.Models.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COF.API.Models.Product
{
    public class ProductSizeFormularModel
    {
        public List<RawMaterialModel> Rms { get; set; }
        public List<ProductFormularForAllSize> Details { get; set; }
    }
}