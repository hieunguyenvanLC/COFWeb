using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Product
{
    public class ProductByCategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductModel> Products { get; set; }
        public ProductByCategoryModel()
        {
            Products = new List<ProductModel>();
        }
    }
    public class ProductModel
    {
        public string Name { get; set; }
        public List<ProductSize> Sizes { get; set;}
    }

    public class ProductSize
    {
        public int SizeId { get; set; }
        public string Size { get; set; }
        public decimal Cost { get; set; }
    }
}
