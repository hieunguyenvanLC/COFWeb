using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class SupplierSearchDto
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? SupplierStatusId { get; set; }
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
