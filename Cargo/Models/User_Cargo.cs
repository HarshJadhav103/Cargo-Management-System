using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cargo.Models
{
    public class User_Cargo
    {
        public int RequestId { get; set; }
        public int QuotationId { get; set; }


        public int UserId { get; set; }
        public string Name { get; set; }
        public int CargoId { get; set; }
        public string CompanyName { get; set; }
        public string WorkCategory { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string RequestDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Description { get; set; }
        public string Item_Name { get; set; }
        public string Qty { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }

        public string Status { get; set; }
    }
}