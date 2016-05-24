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
    public class Vehicles2Controller : Controller
    {
        private VehicleContext db = new VehicleContext();

		// GET: Vehicles2
		public ActionResult Index(string userMessage)
		{
			//return View("Index", db.Vehicles.ToList());
			return RedirectToAction("OldVehicles", new { filterOld = bool.TrueString, userMessage = userMessage });
		}

		public ActionResult SortBy(string sortby, string filterOld, string userMessage)
		{
			if (sortby == string.Empty)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (string.IsNullOrEmpty(filterOld))
			{
				filterOld = bool.TrueString;
			}
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			//List<Vehicle> model = db.Vehicles.ToList();
			List<Vehicle> model = FilterOldVehicles(filterOld, userMessage);
			switch (sortby.ToLower())
			{
				case "parkinglot":
					model.Sort((item1, item2) => item1.ParkingLot.CompareTo(item2.ParkingLot));
					break;
				case "owner":
					model.Sort((item1, item2) => item1.Owner.CompareTo(item2.Owner));
					break;
				case "licensenr":
					model.Sort((item1, item2) => item1.LicenseNr.CompareTo(item2.LicenseNr));
					break;
				case "typeofvehicle":
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

			return View("Index", model);
		}

		public ActionResult OldVehicles(string filterOld, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			return View("Index", FilterOldVehicles(filterOld, userMessage));
		}

		private List<Vehicle> FilterOldVehicles(string filterOld, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			List<Vehicle> model = db.Vehicles.ToList();
			//model = db.Vehicles.Where(item => item.Parked == true).ToList();
			bool excludeOld = true;

			try
			{
				excludeOld = Convert.ToBoolean(filterOld);
			}
			catch (Exception)
			{
			}

			if (excludeOld)
			{
				model.RemoveAll(vehicle => vehicle.Parked == false);
			}

			return model;
		}

		// GET: Vehicles2/Details/5
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

		// GET: Vehicles/Receipt/5
		public ActionResult Receipt(int? id, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vehicle vehicle = db.Vehicles.Find(id);
			if (vehicle == null)
			{
				return HttpNotFound();
			}

			ViewBag.ParkingPeriod = DateTime.Now - vehicle.TimeParked;
			ViewBag.Cost = ((int)((ViewBag.ParkingPeriod - new TimeSpan(0, 1, 0)).TotalHours) + 1) * 60;

			return View(vehicle);
		}

		public ActionResult Search(string Owner, string LicenseNr, string Length, string Weight, string MakeModel, string Color, string NumWheels, string TypeOfVehicle, string Any, string Parked, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			float fLength = -1;
			float fWeight = -1;
			VehicleType vType = VehicleType.None;
			bool bParked = !string.IsNullOrEmpty(Parked);
			int iNumWheels = -1;

			try
			{
				fLength = float.Parse(Length);
			}
			catch (Exception) { }

			try
			{
				fWeight = float.Parse(Weight);
			}
			catch (Exception) { }

			try
			{
				vType = (VehicleType)int.Parse(TypeOfVehicle);
			}
			catch (Exception) { }

			try
			{
				iNumWheels = int.Parse(NumWheels);
			}
			catch (Exception) { }

			var result = db.Vehicles.ToList();
			if (string.IsNullOrEmpty(Any))
			{
				result = result
				.Where(v => string.IsNullOrEmpty(Owner) || v.Owner == Owner)
				.Where(v => string.IsNullOrEmpty(LicenseNr) || v.LicenseNr == LicenseNr)
				.Where(v => fLength == -1 || v.Length == fLength)
				.Where(v => fWeight == -1 || v.Weight == fWeight)
				.Where(v => vType == VehicleType.None || v.TypeOfVehicle == vType)
				.Where(v => v.Parked == bParked || string.IsNullOrEmpty(Parked))
				.Where(v => string.IsNullOrEmpty(Color) || v.Color == Color)
				.Where(v => string.IsNullOrEmpty(MakeModel) || v.MakeAndModel.Contains(MakeModel))
				.Where(v => iNumWheels == -1 || v.NrOfWheels == iNumWheels)
				.ToList();
			}
			else {
				result = result
				.Where(v => v.Owner == Owner
				|| v.LicenseNr == LicenseNr
				|| v.Length == fLength
				|| v.Weight == fWeight
				|| v.MakeAndModel.Contains(MakeModel)
				|| v.Color == Color
				|| vType == VehicleType.None
				|| v.TypeOfVehicle == vType
				|| v.NrOfWheels == iNumWheels)
				.ToList();
			}

			if (Request.IsAjaxRequest())
			{
				return PartialView("_VehicleTable", result);
			}
			return View("_Search", result);
		}

		public ActionResult Statistics(string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			return View(db.Vehicles.ToList());
		}

		// GET: Vehicles2/Create
		public ActionResult Create(string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			return View();
		}

		// POST: Vehicles2/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Owner,LicenseNr,TypeOfVehicle,MakeAndModel,Color,Length,Weight,NrOfWheels")] Vehicle vehicle, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			if (vehicle.TypeOfVehicle == VehicleType.None)
			{
				ViewBag.UserFailMessage = "You cannot select None as a vehicle type";
			}

			else if (ModelState.IsValid)
			{

				vehicle.LicenseNr = vehicle.LicenseNr.Trim().ToUpper();

				if (db.Vehicles.Where(v => v.LicenseNr == vehicle.LicenseNr).FirstOrDefault() != null)
				{
					ViewBag.UserFailMessage = "An other vehicle have the same registation number!";
				}
				else {
					vehicle.TimeParked = DateTime.Now;
					vehicle.Parked = false;
					ViewBag.UserFailMessage = ViewBag.FailedParkMessage = Garage.ParkVehicle(vehicle, db);

					db.Vehicles.Add(vehicle);
					db.SaveChanges();
					return RedirectToAction("Index", new { userMessage = ViewBag.UserFailMessage });
				}
			}

			return View(vehicle);
		}

		// GET: Vehicles2/Edit/5
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

        // POST: Vehicles2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Owner,LicenseNr,TypeOfVehicle,MakeAndModel,Color,Length,Weight,NrOfWheels,TimeParked,Parked,ParkingLot")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles2/Delete/5
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

		public ActionResult TogglePark(int? id, string userMessage)
		{
			if (!String.IsNullOrWhiteSpace(userMessage))
				ViewBag.UserFailMessage = userMessage;

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Vehicle vehicle = db.Vehicles.Find(id);
			bool wasParked = vehicle.Parked;

			if (!vehicle.Parked)
			{
				ViewBag.UserFailMessage = ViewBag.FailedParkMessage = Garage.ParkVehicle(vehicle, db);
			}
			else {
				vehicle.Parked = false;
			}

			db.SaveChanges();

			if (wasParked)
				return Content("/Vehicles/Receipt/" + id);
			return PartialView("_ParkedTableData", vehicle);
		}

		// POST: Vehicles2/Delete/5
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
