using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cargo.Models
{
    public class User_Quotation
    {

        public int CargoId { get; set; }
        public string Email_ID { get; set; }
      
        public string OwnerName { get; set; }
        public string CompanyName { get; set; }
        public string CompAddress { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string WorkCategory { get; set; }

        public string Status { get; set; }
    }
}