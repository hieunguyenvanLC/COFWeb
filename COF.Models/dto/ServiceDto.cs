using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class ServiceDto
    {

        public int ServiceId { get; set; } // ServiceId (Primary key)


        public int SupplierId { get; set; } // SupplierId


        public String ServiceType { get; set; } // ServiceTypeId


        public int ServiceTypeId { get; set; } // ServiceTypeId


        public string Name { get; set; } // Name (length: 50)


        public decimal? Price { get; set; } // Price


        public String ServiceStatus { get; set; } // ServiceStatusId

        public String ServiceStatusId { get; set; } // ServiceStatusId


        public String CreatedDate { get; set; } // CreatedDate


        public string CreatedBy { get; set; } // CreatedBy (length: 30)


        public String UpdatedDate { get; set; } // UpdatedDate


        public string UpdatedBy { get; set; } // UpdatedBy (length: 30)

        public string PriceDisplay { get; set; }
    }
}
