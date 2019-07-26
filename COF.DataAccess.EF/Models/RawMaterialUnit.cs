using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class RawMaterialUnit : BaseEntity
    {
        [MaxLength(256)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RawMaterial> RawMaterials { get; set; }
    }
}
