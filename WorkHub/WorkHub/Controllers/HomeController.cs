using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkHub.Models;

namespace WorkHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET /Home/Index
        [HttpGet]
        public ActionResult Index()
        {
            // If user has successfuly logged in, return view for logged users.
            if (User.Identity.IsAuthenticated) 
            {
                ViewBag.Categories = _db.Categories.ToList();
                var workOrderList = _db.WorkOrders.ToList();
                return View("LoggedIndex", workOrderList);
            }

            // Else return landing page.
            return View();
        }
    }
}