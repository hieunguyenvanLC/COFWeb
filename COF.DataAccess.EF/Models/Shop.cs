using COF.DataAccess.EF.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Shop : BaseEntity, IPartner
    {
        [MaxLength(250)]
        public string ShopName { get; set; }
        [MaxLength(45)]
        public string PhoneNumber { get; set; }
        [MaxLength(250)]
        public string Address { get; set; }
        [MaxLength(250)]
        public string City { get; set; }
        [MaxLength(250)]
        public string State { get; set; }
        [MaxLength(250)]
        public string ZipCode { get; set; }
        public string Description { get; set; }
        [MaxLength(250)]
        public string Status { get; set; }
        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<Table> Tables { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RawMaterial> RawMaterials { get; set; }
        public virtual ICollection<ShopHasUser> ShopHasUsers { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
