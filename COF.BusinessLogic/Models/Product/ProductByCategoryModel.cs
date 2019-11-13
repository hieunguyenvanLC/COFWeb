using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Product
{
    public class ProductByCategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<ProductModel> Products { get; set; }
        public ProductByCategoryModel()
        {
            Products = new List<ProductModel>();
        }
    }
    public class ProductModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<ProductSize> Sizes { get; set;}
        public List<ProductRmUpdateModel> Rms { get; set; }
    }


    

    public class ProductSize
    {
        public int Id { get; set; }
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

    public class ProductCreateModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int ShopId { get; set; }
        public int PartnerId { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public List<ProductRmUpdateModel> Rms { get; set; }
    }

    public class ProductFormularForAllSize
    {
        public int ProductSizeId { get; set; }
        public int SizeId { get; set; }
        public string Size { get; set; }
        public List<ProductRmFormularDetailModel> Formulars { get; set; }
        
    }
    public class ProductRmFormularDetailModel
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public string RawMaterialName { get; set; }
        public decimal Amount { get; set; }
    }


    public class ProductRmUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductSizeRequestModel
    {

        public int SizeId { get; set; }
        public int ProductId { get; set; }
        public Decimal Price { get; set; }
    }

    public enum ProductFilterType
    {
        Shop = 1,
        Keyword = 2
    }

   
}
