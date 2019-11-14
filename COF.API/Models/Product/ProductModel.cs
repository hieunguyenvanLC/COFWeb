using COF.BusinessLogic.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COF.API.Models.Product
{
   public class ProductCreateModel
   {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(250,ErrorMessage ="Tên sản phẩm có độ dài tối đa là 250 kí tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Chi nhánh là bắt buộc.")]
        public int ShopId { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Thành phần nguyên liệu là bắt buộc.")]
        public List<ProductRmUpdateModel> Rms { get; set; } 
    }
    
    

    public class ProductSizeCreateModel
    {
        
        [Required(ErrorMessage = "Size là bắt buộc.")]
        public int  SizeId{ get; set; }
        [Required(ErrorMessage = "Sản phẩm là bắt buộc.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Giá tiền là bắt buộc")]
        public Decimal Price { get; set; }
    }

    public class ProductSizeUpdateModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Size là bắt buộc.")]
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Sản phẩm là bắt buộc.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Giá tiền là bắt buộc")]
        public Decimal Price { get; set; }
    }
}