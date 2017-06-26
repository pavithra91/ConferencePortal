using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.App_Code
{
    public class ShoppingCart
    {
        public string ConventionID { get; set; }
        public int ClientId { get; set; }
        public int NoofDelegates { get; set; }
        public Client client { get; set; }
        public List<RoomsInCart> Rooms = new List<RoomsInCart>();
        public List<TransportInCart> Transport = new List<TransportInCart>();
        public List<ExcursionsInCart> Excursion = new List<ExcursionsInCart>();
        public double TotalPrice { get; set; }
    }
}