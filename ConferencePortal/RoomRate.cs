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
    
    public partial class RoomRate
    {
        public int AUTOID { get; set; }
        public Nullable<int> ConventionID { get; set; }
        public Nullable<System.DateTime> RateDate { get; set; }
        public Nullable<int> RoomID { get; set; }
        public Nullable<int> Allotment { get; set; }
        public Nullable<int> Occupancy { get; set; }
        public Nullable<double> Rate { get; set; }
    
        public virtual Configuration Configuration { get; set; }
        public virtual Room Room { get; set; }
    }
}
