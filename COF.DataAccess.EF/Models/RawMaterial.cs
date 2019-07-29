using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class RawMaterial : BaseEntity, IPartner
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AutoTotalQty { get; set; }
        public int RawMaterialUnitId { get; set; }
        public int UserInputTotalQty { get; set; }
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual RawMaterialUnit RawMaterialUnit { get; set; }

        public virtual ICollection<ProductHasRawMaterial> ProductHasRawMaterials { get; set; }
        public virtual ICollection<RawMaterialHistory> RawMaterialHistories { get; set; }
        public int PartnerId { get; set ; }
    }
}
