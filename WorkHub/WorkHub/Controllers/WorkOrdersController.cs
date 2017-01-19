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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkOrders
        [Authorize]
        public async Task<ActionResult> Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            var workOrders = db.WorkOrders.Where(x => x.IsActive).Include(w => w.Category).Include(w => w.User);
            return View(await workOrders.ToListAsync());
        }

        // GET: WorkOrders/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.Categories = db.Categories.ToList();
            try
            {
                var workOrders = db.WorkOrders.Include(w => w.Category).Include(w => w.User);
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
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Type");
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WorkOrder workOrder)
        {
            ViewBag.Categories = db.Categories.ToList();
            workOrder.AddDate = DateTime.Now;
            workOrder.IsActive = true;
            workOrder.IsCompleted = false;
            workOrder.UserRefId = User.Identity.GetUserId();

       

            if (ModelState.IsValid)
            {
                db.WorkOrders.Add(workOrder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Categories = db.Categories.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserRefId,CategoryId,IsActive,IsCompleted,AddDate,Payment,Location,Description,PhoneNumber")] WorkOrder workOrder)
        {
            ViewBag.Categories = db.Categories.ToList();
            if (ModelState.IsValid)
            {
                db.Entry(workOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Type", workOrder.CategoryId);
            ViewBag.UserRefId = new SelectList(db.Users, "Id", "Address", workOrder.UserRefId);
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.Categories = db.Categories.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Categories = db.Categories.ToList();
            WorkOrder workOrder = await db.WorkOrders.FindAsync(id);
            db.WorkOrders.Remove(workOrder);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
