using System;
using System.ComponentModel.DataAnnotations;

namespace COF.BusinessLogic.Models.KiotViet.Customers
{
    public class KiotVietCustomerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LocationName { get; set; }
        public string WardName { get; set; }
        public string AddressDatail => Address + " - " + WardName + " - " + LocationName;
        public DateTime? BirthDate { get; set; }
    }

    public class CustomerCreateModel
    {
        [Required]
        public string FullName { get; set; }
        public int ShopId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string Email { get; set; }
    }

    public class CustomerRegisterModel : CustomerCreateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int PartnerId { get; set; } = 1;
        public string Code { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
