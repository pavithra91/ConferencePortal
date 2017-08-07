using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class ConfigModel
    {
        public Hotel _hotel { get; set; }
        public List<Hotel> hotelList { get; set; }
    }
}