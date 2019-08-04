using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COF.API.Models.Category
{
    public class CategoryCreateModel
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Chi nhánh là bắt buộc.")]
        public int ShopId { get; set; }
    }

    public class CategoryUpdateModel
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Chi nhánh là bắt buộc.")]
        public int ShopId { get; set; }
    }
}