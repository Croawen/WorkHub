using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class WorkOrder
    {
        [Key]
        public int Id { get; set; }

        public string UserRefId { get; set; }

        [ForeignKey("UserRefId")]
        public ApplicationUser User { get; set; }
        
        public bool IsActive { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime AddDate { get; set; }

        public double Payment { get; set; }

        public GeoLocation Location { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }
    }
}