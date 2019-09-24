using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COF.UserWeb.Models
{
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public string Product { set; get; }
        public string ImageUrl { get; set; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public ProductSizeViewModel Size { get; set; }
        public List<ProductSizeViewModel> AllSizes { get; set; }
    }

    public class ProductSizeViewModel
    {
        public int ProductSizeId { get; set; }
        public string Size { get; set; }
        public int SizeId { get; set; }
    }
}