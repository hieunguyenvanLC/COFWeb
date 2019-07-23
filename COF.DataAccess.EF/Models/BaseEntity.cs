using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        [MaxLength(250)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        [MaxLength(250)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
