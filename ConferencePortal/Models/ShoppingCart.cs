using ConferencePortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
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

        public string ItemType { get; set; }
        public string ItemID { get; set; }
        public string Rate { get; set; }
        public string Count { get; set; }
    }

    public class RoomsInCart
    {
        public Room room { get; set; }
        public double Price { get; set; }
        public int NoofRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<Deligate> Deligate { get; set; }
    }

    public class TransportInCart
    {
        public TransportRate TR { get; set; }
        public int NoOfVehicles { get; set; }
        public double Price { get; set; }
        public string TransportType { get; set; }
        public string PickUpDate { get; set; }
        public string PickUpTime { get; set; }
        public string DropOffDate { get; set; }
        public string DropOffTime { get; set; }
    }

    public class ExcursionsInCart
    {
        public Excursion Excursion { get; set; }
        public DateTime ExcursionDate { get; set; }
        public int NoOfAdults { get; set; }
        public double Price { get; set; }
    }
}