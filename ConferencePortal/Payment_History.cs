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
    
    public partial class Payment_History
    {
        public int AUTOID { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
        public string BookingID { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public Nullable<double> PartialPaymentAmount { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
    
        public virtual Payment Payment { get; set; }
    }
}
