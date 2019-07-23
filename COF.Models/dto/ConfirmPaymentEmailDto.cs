using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class ConfirmPaymentEmailDto
    {
        public string username { get; set; }
        public string password { get; set; }
        public string paymentEmail { get; set; }
    }
}
