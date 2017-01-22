using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class Category
    {
        // Primary Key
        [Key]
        public int CategoryId { get; set; }

        public string Type { get; set; }
    }
}