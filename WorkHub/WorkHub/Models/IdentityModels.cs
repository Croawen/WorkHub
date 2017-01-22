using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace WorkHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        public int ReputationPoints { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Collection of user's workorders.
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // DbSets used to make specific tables
        public System.Data.Entity.DbSet<WorkHub.Models.WorkOrder> WorkOrders { get; set; }
        public System.Data.Entity.DbSet<WorkHub.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<WorkHub.Models.Settings> Settings { get; set; }
    }
}