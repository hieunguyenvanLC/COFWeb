using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Models.Customer
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal ActiveBonusPoint { get; set; }
        public string BonusLevel { get; set; }
    }
    public class CustomerCreateModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public DateTime? BirthDate { get; set; }
    }
    public class CustomerSearchPagingModel
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string JoinDate => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd-MM-yyyy HH:mm") : "";
        public string CustomerLevel { get; set; }
        public decimal ActiveBonusPoint { get; set; }
        public string Code { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateToString => BirthDate.HasValue ? BirthDate.Value.ToString("dd-MM-yyyy") : "";
        public bool ToDayIsBirhDay => BirthDate.HasValue && BirthDate.Value.Month == DateTime.Now.Month && BirthDate.Value.Day == DateTime.Now.Day;
        public long? RowCounts { get; set; }
    }
}
