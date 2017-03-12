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
    
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            this.Reservations = new HashSet<Reservation>();
        }
    
        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string RoomName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public Nullable<int> ConferenceID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> AllotmentID { get; set; }
        public Nullable<int> CurCode { get; set; }
        public Nullable<int> OccupancyID { get; set; }
        public string RoomImage { get; set; }
    
        public virtual Allotment Allotment { get; set; }
        public virtual Conference Conference { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Hotel Hotel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}