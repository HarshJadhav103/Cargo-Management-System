
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cargo
{
    using System;
    using System.Collections.Generic;

    public partial class User_Cargo_Request
    {
        public int RequestId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CargoId { get; set; }
        public string WorkCategory { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string RequestDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}