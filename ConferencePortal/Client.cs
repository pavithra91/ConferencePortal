//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConferencePortal
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public int ClientID { get; set; }
        public Nullable<int> ConferenceID { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string Address { get; set; }
        public string country { get; set; }
        public string BookingID { get; set; }
        public string City { get; set; }
        public string Deligate { get; set; }
        public string ContactNumber { get; set; }
        public string email { get; set; }
        public string verified { get; set; }
    
        public virtual Conference Conference { get; set; }
    }
}