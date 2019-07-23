using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class SupplierDto
    {

        public int SupplierId { get; set; } // SupplierId (Primary key)

        public string Name { get; set; } // Name (length: 50)

        public string Avatar { get; set; } // Avatar (length: 200)


        public string SupplierStatus { get; set; } // SupplierStatusId

        public int? SupplierStatusId { get; set; }

        public string CreatedDate { get; set; } // CreatedDate

        public string Email { get; set; }

        public int? MainBranchId { get; set; } // MainBranchId

        public string PhoneNumber { get; set; } // PhoneNumber (length: 11)

        public List<ServiceDto> Services { get; set; }

        public List<BranchDto> Branches { get; set; }

        public string Description { get; set; }
        public string PaymentEmail { get; set; }


    }
}
