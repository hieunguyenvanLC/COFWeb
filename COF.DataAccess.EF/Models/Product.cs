using COF.DataAccess.EF.Configurations;
using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Product : BaseEntity
    {
        [MaxLength(45)]
        public string ProductName { get; set; }

        public decimal Cost { get; set; }

        [MaxLength(45)]
        public string Description { get; set; }

        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductHasRawMaterial> ProductHasRawMaterials { get; set; }
    }
}
