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
    public class Product : BaseEntity , IPartner
    {
        [MaxLength(256)]
        public string ProductName { get; set; }

        public string Description { get; set; }

        public int ShopId { get; set; }

        public int PartnerId { get; set; }
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<ProductHasRawMaterial> ProductHasRawMaterials { get; set; }
        public string ProductImage { get; set; }
    }

    public class ProductSize : BaseEntity
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
        public decimal Cost { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class Size : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
