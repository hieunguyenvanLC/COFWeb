using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class BranchDto
    {
        public int BranchId { get; set; } // BranchId (Primary key)

        public string Name { get; set; } // Name (length: 50)

        public int? SupplierId { get; set; } // SupplierId

        public int? CityId { get; set; } // CityId

        public String City { get; set; }


        public int? DistrictId { get; set; } // DistrictId

        public String District { get; set; }

        public string Address { get; set; } // Address (length: 50)

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public string GoogleMapSearchKey { get; set; }
    }
}
