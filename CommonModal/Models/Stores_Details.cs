//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonModal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stores_Details
    {
        public int Sno { get; set; }
        public System.Guid StoreUniqueId { get; set; }
        public string StoreName { get; set; }
        public string Password { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMailId { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Alternet_No { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int Location_Code { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Type { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Rating { get; set; }
    }
}
