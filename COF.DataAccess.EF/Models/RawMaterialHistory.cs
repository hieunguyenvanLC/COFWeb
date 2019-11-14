using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.DataAccess.EF.Models
{
    public class RawMaterialHistory : BaseEntity, IPartner
    {
        public DateTime TimeAccess { get; set; }
        public TransactionType TransactionTypeId { get; set; }
        public decimal Quantity { get; set; }
        public InputType InputTypeId { get; set; }
        public int RawMaterialId { get; set; }
        public RawMaterial RawMaterial { get; set; }
        public decimal TotalQtyAtTimeAccess { get; set; }
        [NotMapped]
        public decimal OldQty
        {
            get
            {
                if (TransactionTypeId == TransactionType.Increasement)
                {
                    return TotalQtyAtTimeAccess - Quantity;
                }
                else
                {
                    return TotalQtyAtTimeAccess + Quantity;
                }
            }
        }
        public int PartnerId { get; set; }
        public string Description { get; set; }
    }
    public enum TransactionType
    {
        Increasement = 1,
        Decreasement = 2
    }
    public enum InputType
    {
        UserInput = 1,
        Auto = 2
    }
}
