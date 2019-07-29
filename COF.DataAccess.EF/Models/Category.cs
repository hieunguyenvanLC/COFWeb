using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class Category : BaseEntity, IPartner
    {
        [MaxLength(256)]
        public string Name { get; set; }
        public int SeqNo { get; set; }
        public int ShopId { get; set; }
        public int PartnerId { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
