using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class ProductHasRawMaterial : BaseEntity, IPartner
    {
        public int RawMaterialId { get; set; }
        public int ProductId { get; set; }
        public int RawMaterialUnitId { get; set; }

        public virtual RawMaterial RawMaterial { get; set; }
        public virtual Product Product { get; set; }
        public int PartnerId { get; set; }
    }
}
