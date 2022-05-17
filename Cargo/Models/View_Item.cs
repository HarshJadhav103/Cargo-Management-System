using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cargo.Models
{
    public class View_Item
    {
        public int CargoId { get; set; }
        public string Email_ID { get; set; }
        public string CompanyName { get; set; }
        public int RequestId { get; set; }
        public string WorkCategory { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string RequestDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Description { get; set; }
        public int ItemId { get; set; }
        public string Item_Name { get; set; }
        public string Qty { get; set; }

        public string Status { get; set; }



    }
}