using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WorkHub.Models;

[assembly: OwinStartupAttribute(typeof(WorkHub.Startup))]
namespace WorkHub
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
            Seed();
        }

        private void CreateRolesandUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole {Name = "Admin"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole {Name = "User"};
                roleManager.Create(role);
            }
        }

        private void Seed()
        {
            if (!db.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {Type = "Gardening"},
                    new Category {Type = "Cooking"},
                    new Category {Type = "Repair"},
                    new Category {Type = "Coding"},
                    new Category {Type = "Cleaning"},
                    new Category {Type = "Teaching"}
                };

                foreach(var category in categories)
                {
                    db.Categories.AddOrUpdate(category);
                }
            }

           if(!db.Settings.Any())
            {
                var setting = new Settings { LayoutType = 1 };
                db.Settings.AddOrUpdate(setting);
            }

            db.SaveChanges();
        }
    }
}
