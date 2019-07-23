using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class ServiceCreateDto
    {
        public int SupplierId { get; set; }
        public int ServiceTypeId { get; set; }
        public String Name { get; set; }
        public String Price { get; set; }
        public int ServiceStatusId { get; set; }
    }
}
