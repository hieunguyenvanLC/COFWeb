using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Models.dto
{
    public class FeedbackDto
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? OrderId { get; set; }
        public int NumberOfStar { get; set; }
        public string FeedbackContent { get; set; }
    }
}
