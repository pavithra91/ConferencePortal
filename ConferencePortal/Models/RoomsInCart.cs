using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class RoomsInCart
    {
        public Room room { get; set; }
        public double Price { get; set; }
        public int NoofRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<Registration> Deligate { get; set; }
    }
}