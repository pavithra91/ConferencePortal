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

        public List<Room> RateAlreadyAssign { get; set; }

        public HotelDescription _hotelDescription { get; set; }
        public List<HotelDescription> _hotelDescriptionList { get; set; }
        public RoomDescription _roomDescription { get; set; }
        public List<RoomDescription> _roomDescriptionList { get; set; }

        public int ID { get; set; }
        public int DescID { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

    }

    public class TariffModel
    {
        public bool _showDeleted { get; set; }
        public Room _room { get; set; }
        public RoomRate _roomRate { get; set; }
        public List<RoomRate> _roomRateList { get; set; }

        public int ID { get; set; }
        public int RoomID { get; set; }
        public int Allotment { get; set; }
        public DateTime RateDate { get; set; }
        public DateTime EndDate { get; set; }
        public double RoomRate { get; set; }
    }
}