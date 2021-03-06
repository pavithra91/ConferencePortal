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
    
    public partial class Transport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transport()
        {
            this.Reservations = new HashSet<Reservation>();
            this.TransportRates = new HashSet<TransportRate>();
            this.TransportReservations = new HashSet<TransportReservation>();
        }
    
        public int TransportID { get; set; }
        public Nullable<int> ConventionID { get; set; }
        public string StartLocation { get; set; }
        public string DropOffLocation { get; set; }
        public Nullable<int> CurCode { get; set; }
        public string ShowInSearch { get; set; }
        public string Type { get; set; }
    
        public virtual Configuration Configuration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransportRate> TransportRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransportReservation> TransportReservations { get; set; }
    }
}
