using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Hotel> _hotel { get; set; }
        public IEnumerable<ConventionHotel> _conventionHotel { get; set; }
        public IEnumerable<Room> _room { get; set; }
        public IEnumerable<RoomAllotment> _roomAllotment { get; set; }
        public IEnumerable<TransportRate> _transportRate { get; set; }
        public IEnumerable<Excursion> _excursion { get; set; }
        public IEnumerable<Configuration> _configuration { get; set; }

    }
}