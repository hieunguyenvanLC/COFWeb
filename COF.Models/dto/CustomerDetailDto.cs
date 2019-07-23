using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class CustomerDetailDto
    {
        public int CustomerId { get; set; }
        public String FullName { get; set; }
        public String Address { get; set; }
        public int? Age { get; set; }
        public String PhoneNumber { get; set; }
        public AccountDetailDto accountDetailDto { get; set; }
    }
    public class AccountDetailDto
    {
        public int AccountId { get; set; }
        public String Username { get; set; }
        public String CreatedDate { get; set; }
    }
}
