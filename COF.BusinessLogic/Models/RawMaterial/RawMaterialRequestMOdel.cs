using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        public decimal Qty { get; set; }
        public string Note { get; set; }
    }

    public class RmHistorySearchModel
    {
        public int Id { get; set; }
        public int PageIndex { get; set; } = 15;
        public int PageSize { get; set; } = 1;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool? IsAuto { get; set; }

        public DateTime? _fromDate
        {
            get
            {
                if (!string.IsNullOrEmpty(StartDate))
                {
                    return DateTime.ParseExact(StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                return null;
            }
        }
        public DateTime? _toDate
        {
            get
            {
                if (!string.IsNullOrEmpty(EndDate))
                {
                    return DateTime.ParseExact(EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                return null;
            }
        }
    }
}
