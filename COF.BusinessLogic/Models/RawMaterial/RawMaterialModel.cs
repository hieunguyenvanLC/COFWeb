using COF.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.RawMaterial
{
    public class RawMaterialModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AutoTotalQty { get; set; }
        public int RawMaterialUnitId { get; set; }
        public string RawMaterialUnitName { get; set; }
        public int UserInputTotalQty { get; set; }
        public int ShopId { get; set; }
        public string Shop { get; set; }
        public long? RowCounts { get; set; }
    }

    public class RawMaterialUnitModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RawMaterialDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal AutoTotalQty { get; set; }
        public int RawMaterialUnitId { get; set; }
        public string RawMaterialUnitName { get; set; }
        public decimal UserInputTotalQty { get; set; }
        public int ShopId { get; set; }
        public string Shop { get; set; }
    }

    public class RawMaterialHistoryDetailModel
    {
        public DateTime TimeAccess { get; set; }
        public TransactionType TransactionTypeId { get; set; }
        public int Quantity { get; set; }
        public InputType InputTypeId { get; set; }
        public int RawMaterialId { get; set; }
        public int TotalQtyAtTimeAccess { get; set; }
        public int OldQty
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
        public long? RowCounts { get; set; }
    }
}
