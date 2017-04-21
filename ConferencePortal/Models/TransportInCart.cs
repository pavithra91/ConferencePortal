using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
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
}