using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2.DataAccessLayer;
using Garage2.Models;

namespace Garage2.Controllers
{
    public class VehiclesController : Controller
    {
        private VehicleContext db = new VehicleContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            return View("OverView", db.Vehicles.ToList());
        }

        public ActionResult SortBy(string sortby)
        {
            if (sortby == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<Vehicle> model = db.Vehicles.ToList();
            switch (sortby.ToLower())
            {
                case "owner":
                    model.Sort((item1, item2) => item1.Owner.CompareTo(item2.Owner));
                    break;
                case "Licensenr":
                    model.Sort((item1, item2) => item1.LicenseNr.CompareTo(item2.LicenseNr));
                    break;
                case "TypeOfVehicle":
                    model.Sort((item1, item2) => item1.TypeOfVehicle.CompareTo(item2.TypeOfVehicle));
                    break;
                case "length":
                    model.Sort((item1, item2) => item1.Length.CompareTo(item2.Length));
                    break;
                case "weight":
                    model.Sort((item1, item2) => item1.Weight.CompareTo(item2.Weight));
                    break;
                case "timeparked":
                    model.Sort((item1, item2) => item1.TimeParked.CompareTo(item2.TimeParked));
                    break;
                case "parked":
                    model.Sort((item1, item2) => item1.Parked.CompareTo(item2.Parked));
                    break;
                default:
                    break;
            }

            return View("OverView", model);
        }
    
        
        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Owner,LicenseNr,TypeOfVehicle,Length,Weight,Parked")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.TimeParked = DateTime.Now;
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Owner,LicenseNr,TypeOfVehicle,Length,Weight,TimeParked,Parked")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
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
