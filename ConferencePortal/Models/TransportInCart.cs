using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class TransportInCart
    {
        public Transport TR { get; set; }
        public int NoOfVehicles { get; set; }
        public double Price { get; set; }
    }
}