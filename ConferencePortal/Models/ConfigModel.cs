using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class ConfigModel
    {
        public Hotel _hotel { get; set; }
        public Room _room { get; set; }
        public List<Hotel> hotelList { get; set; }
        public List<Room> roomList { get; set; }

        public HotelDescription _hotelDescription { get; set; }
        public List<HotelDescription> _hotelDescriptionList { get; set; }
        public RoomDescription _roomDescription { get; set; }
        public List<RoomDescription> _roomDescriptionList { get; set; }

        public int HotelID { get; set; }        
        public int DescID { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

    }

    //public class HotelModel
    //{
    //    public Hotel _hotel { get; set; }
    //    public 
    //}
}