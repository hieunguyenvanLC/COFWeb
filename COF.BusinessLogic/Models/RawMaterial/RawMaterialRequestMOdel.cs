using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.RawMaterial
{
    public class RawMaterialRequestModel
    {
        [Required(ErrorMessage = "Tên nguyên liệu là bắt buộc")]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedByUser { get; set; }
        public string CreateByUserId { get; set; }
        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        public int RawMaterialUnitId { get; set; }
        [Required(ErrorMessage = "Chi nhánh là bắt buộc")]
        public int ShopId { get; set; }
    }

    public class RmUpdateQtyModel
    {
        public int Id { get; set; }
        public int Qty { get; set; }
    }
}
