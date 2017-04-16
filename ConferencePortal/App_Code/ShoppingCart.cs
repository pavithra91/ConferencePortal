using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.App_Code
{
    public class ShoppingCart
    {
        public int ClientId { get; set; }
        public int NoofDelegates { get; set; }
        public Client client { get; set; }
        public List<RoomsInCart> Rooms = new List<RoomsInCart>();
        public List<Transport> Transport = new List<Transport>();
    }
}