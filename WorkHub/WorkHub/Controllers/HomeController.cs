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
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult LoggedIndex()
        {
            ViewBag.Categories = db.Categories.ToList();
            var workOrderList = db.WorkOrders.ToList();
            return View(workOrderList);
        }
    }
}