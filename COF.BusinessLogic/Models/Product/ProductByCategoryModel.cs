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
        public string Description { get; set; }
        public List<ProductSize> Sizes { get; set;}
    }

    public class ProductSize
    {
        public int SizeId { get; set; }
        public string Size { get; set; }
        public decimal Cost { get; set; }
    }

    public class ProductByShop
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        public List<ProductByCategoryModel> Categories {get;set;}
        public ProductByShop()
        {
            this.Categories = new List<ProductByCategoryModel>();
        }
    }

    public class ProductFilterModel
    {
        public ProductFilterType filterType { get; set; }
        public string Value { get; set; }
    }

    public enum ProductFilterType
    {
        Shop = 1,
        Keyword = 2
    }
}
