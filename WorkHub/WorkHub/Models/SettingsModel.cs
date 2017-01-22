using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkHub.Models
{
    public class Settings
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        public int LayoutType { get; set; }
    }
}