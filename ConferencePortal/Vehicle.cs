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
    
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.TransportRates = new HashSet<TransportRate>();
        }
    
        public int VehicleID { get; set; }
        public string VehicleCategory { get; set; }
        public Nullable<int> NoOfPassengers { get; set; }
        public string Image { get; set; }
        public Nullable<int> NoOfDoors { get; set; }
        public Nullable<int> NoOfBaggages { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransportRate> TransportRates { get; set; }
    }
}
