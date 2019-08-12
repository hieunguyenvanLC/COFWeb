using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COF.API.Models.Customer
{
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

    public class CustomerCreateResultModel
    {
        public int CustomerId { get; set; }
    }
}