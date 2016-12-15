﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WorkHub.Models;

namespace WorkHub.Controllers
{
    public class WorkOrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/WorkOrders
        public IQueryable<WorkOrder> GetWorkOrders()
        {
            return db.WorkOrders;
        }

        // GET: api/WorkOrders/5
        [ResponseType(typeof(WorkOrder))]
        public async Task<IHttpActionResult> GetWorkOrder(int id)
        {
            WorkOrder workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
        }

        // PUT: api/WorkOrders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWorkOrder(int id, WorkOrder workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workOrder.Id)
            {
                return BadRequest();
            }

            db.Entry(workOrder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WorkOrders
        [ResponseType(typeof(WorkOrder))]
        public async Task<IHttpActionResult> PostWorkOrder(WorkOrder workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WorkOrders.Add(workOrder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = workOrder.Id }, workOrder);
        }

        // DELETE: api/WorkOrders/5
        [ResponseType(typeof(WorkOrder))]
        public async Task<IHttpActionResult> DeleteWorkOrder(int id)
        {
            WorkOrder workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }

            db.WorkOrders.Remove(workOrder);
            await db.SaveChangesAsync();

            return Ok(workOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkOrderExists(int id)
        {
            return db.WorkOrders.Count(e => e.Id == id) > 0;
        }
    }
}