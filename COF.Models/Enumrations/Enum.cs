using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.Enumrations
{
    public class Enum
    {
        public enum AccountType
        {
            Admin = 1,
            Supplier = 2,
            Customer = 3
        }
        public enum SupplierStatusEnum
        {
            Active = 1,
            Inactive = 2,
            WaitingReview = 3,
            ReviewOK = 5
        }

        public enum PromotionStatus
        {
            Active = 1,
            Inactive = 2
        }

        public enum ServiceStatus
        {
            Active = 1,
            Inactive = 2
        }

        public enum OrderStatus
        {
            InProcessing = 1,
            Done = 2,
            Cancel = 3
        }
    }
}
