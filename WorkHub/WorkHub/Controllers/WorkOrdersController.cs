using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WorkHub.Models;

namespace WorkHub.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: WorkOrders
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.Categories = _db.Categories.ToList();

            // .Include allows us to use value behind foreign keys.
            var workOrders = _db.WorkOrders.Where(x => x.IsActive).Include(w => w.Category).Include(w => w.User);
            return View(await workOrders.ToListAsync());
        }

        // GET: WorkOrders/Details/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.Categories = _db.Categories.ToList();
            try
            {
                var workOrders = _db.WorkOrders.Include(w => w.Category).Include(w => w.User);
                WorkOrder workOrder = await workOrders.FirstOrDefaultAsync(x => x.Id == id);

                if (workOrder == null)
                {
                    return HttpNotFound();
                }
                return View(workOrder);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
              
        }

        // GET: WorkOrders/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Type");
            return View();
        }

        // GET: WorkOrders/Category
        [Authorize]
        [HttpGet]
        public ActionResult Category(int id)
        {
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.WorkOrders = _db.WorkOrders.Where(x => x.Category.CategoryId == id).ToList();
            return View();
        }

        // POST: WorkOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WorkOrder workOrder)
        {
            ViewBag.Categories = _db.Categories.ToList();
            workOrder.AddDate = DateTime.Now;
            workOrder.IsActive = true;
            workOrder.IsCompleted = false;
            workOrder.UserRefId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                _db.WorkOrders.Add(workOrder);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(_db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Categories = _db.Categories.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _db.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(_db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WorkOrder workOrder)
        {
            ViewBag.Categories = _db.Categories.ToList();
            workOrder.AddDate = DateTime.Now;
            workOrder.IsActive = true;
            workOrder.IsCompleted = false;
            workOrder.UserRefId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                _db.Entry(workOrder).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(_db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Categories = _db.Categories.ToList();
            WorkOrder workOrder = await _db.WorkOrders.FindAsync(id);
            _db.WorkOrders.Remove(workOrder);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
