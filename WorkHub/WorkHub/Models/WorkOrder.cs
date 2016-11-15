using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class WorkOrder
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Payment { get; set; }
        public string PhoneNumber { get; set; }
    }
}