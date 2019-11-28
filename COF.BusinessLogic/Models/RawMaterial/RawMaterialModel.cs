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
        public decimal AutoTotalQty { get; set; }
        public int RawMaterialUnitId { get; set; }
        public string RawMaterialUnitName { get; set; }
        public decimal UserInputTotalQty { get; set; }
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
        public string TimeAccessDisplay => TimeAccess.ToString("dd-MM-yyyy HH:mm:ss");
        public TransactionType TransactionTypeId { get; set; }
        public decimal Quantity { get; set; }
        public InputType InputTypeId { get; set; }
        public decimal RawMaterialId { get; set; }
        public decimal TotalQtyAtTimeAccess { get; set; }
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
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public long? RowCounts { get; set; }
    }

    public class TodayRawMaterialReport
    {
        public int RmId { get; set; }
        public string RmName { get; set; }
        public int TotalOrder { get; set; }
        public decimal OrderUsedAmount { get; set; }
        public decimal StartDayAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public int TotalUsedRms { get; set; }
    }
}
