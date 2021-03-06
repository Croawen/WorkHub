﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class WorkOrder
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // UserRefId is a foreign key for ApplicationUser object
        public string UserRefId { get; set; }
        [ForeignKey("UserRefId")]
        public ApplicationUser User { get; set; }
        
        // CategoryId is a foreign key for Category object
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public bool IsActive { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime AddDate { get; set; }

        public double Payment { get; set; }

        // Latitude and Longitude for Google Maps
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }
    }
}