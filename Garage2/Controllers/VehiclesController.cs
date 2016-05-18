﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            //return View("Index", db.Vehicles.ToList());
            return RedirectToAction( "OldVehicles", new { filterOld = bool.TrueString } );

        }

        public ActionResult SortBy( string sortby, string filterOld )
        {
            if ( sortby == string.Empty ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            if ( string.IsNullOrEmpty( filterOld ) ) {
                filterOld = bool.TrueString;
            }

            //List<Vehicle> model = db.Vehicles.ToList();
            List<Vehicle> model = FilterOldVehicles( filterOld );
            switch ( sortby.ToLower() ) {
                case "parkinglot":
                    model.Sort((item1, item2) => item1.ParkingLot.CompareTo(item2.ParkingLot));
                    break;
                case "owner":
                    model.Sort( ( item1, item2 ) => item1.Owner.CompareTo( item2.Owner ) );
                    break;
                case "licensenr":
                    model.Sort( ( item1, item2 ) => item1.LicenseNr.CompareTo( item2.LicenseNr ) );
                    break;
                case "typeofvehicle":
                    model.Sort( ( item1, item2 ) => item1.TypeOfVehicle.CompareTo( item2.TypeOfVehicle ) );
                    break;
                case "length":
                    model.Sort( ( item1, item2 ) => item1.Length.CompareTo( item2.Length ) );
                    break;
                case "weight":
                    model.Sort( ( item1, item2 ) => item1.Weight.CompareTo( item2.Weight ) );
                    break;
                case "timeparked":
                    model.Sort( ( item1, item2 ) => item1.TimeParked.CompareTo( item2.TimeParked ) );
                    break;
                case "parked":
                    model.Sort( ( item1, item2 ) => item1.Parked.CompareTo( item2.Parked ) );
                    break;
                default:
                    break;
            }

            return View( "Index", model );
        }

        public ActionResult OldVehicles( string filterOld )
        {
            return View( "Index", FilterOldVehicles( filterOld ) );
        }

        private List<Vehicle> FilterOldVehicles( string filterOld )
        {
            List<Vehicle> model = db.Vehicles.ToList();
            //model = db.Vehicles.Where(item => item.Parked == true).ToList();
            bool excludeOld = true;

            try {
                excludeOld = Convert.ToBoolean( filterOld );
            }
            catch ( Exception ) {
            }

            if ( excludeOld ) {
                model.RemoveAll( vehicle => vehicle.Parked == false );
            }

            return model;
        }

        // GET: Vehicles/Details/5
        public ActionResult Details( int? id )
        {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Vehicle vehicle = db.Vehicles.Find( id );
            if ( vehicle == null ) {
                return HttpNotFound();
            }
            return View( vehicle );
        }

        // GET: Vehicles/Receipt/5
        public ActionResult Receipt( int? id )
        {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Vehicle vehicle = db.Vehicles.Find( id );
            if ( vehicle == null ) {
                return HttpNotFound();
            }

            ViewBag.ParkingPeriod = DateTime.Now - vehicle.TimeParked;
            ViewBag.Cost = ((int)((ViewBag.ParkingPeriod - new TimeSpan( 0, 1, 0 )).TotalHours) + 1) * 60;

            return View( vehicle );
        }

        public ActionResult Search( string Owner, string LicenseNr, string Length, string Weight, string TypeOfVehicle, string Any, string Parked )
        {
            float fLength = -1;
            float fWeight = -1;
            VehicleType vType = VehicleType.None;
            bool bParked = !string.IsNullOrEmpty( Parked );

            try {
                fLength = float.Parse( Length );
            }
            catch ( Exception ) { }

            try {
                fWeight = float.Parse( Weight );
            }
            catch ( Exception ) { }

            try {
                vType = (VehicleType)int.Parse( TypeOfVehicle );
            }
            catch ( Exception ) { }

            var result = db.Vehicles.ToList();
            if ( string.IsNullOrEmpty( Any ) ) {
                result = result
                .Where( v => string.IsNullOrEmpty( Owner ) || v.Owner == Owner )
                .Where( v => string.IsNullOrEmpty( LicenseNr ) || v.LicenseNr == LicenseNr )
                .Where( v => fLength == -1 || v.Length == fLength )
                .Where( v => fWeight == -1 || v.Weight == fWeight )
                .Where( v => vType == VehicleType.None || v.TypeOfVehicle == vType )
                .Where( v => v.Parked == bParked || string.IsNullOrEmpty( Parked ) )
                .ToList();
            }
            else {
                result = result
                .Where( v => v.Owner == Owner
                 || v.LicenseNr == LicenseNr
                 || v.Length == fLength
                 || v.Weight == fWeight
                 || vType == VehicleType.None
                 || v.TypeOfVehicle == vType )
                .ToList();
            }

            if ( Request.IsAjaxRequest() ) {
                return PartialView( "_VehicleTable", result );
            }
            return View( "_Search", result );
        }

        public ActionResult Statistics()
        {
            return View(db.Vehicles.ToList());
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
        public ActionResult Create( [Bind( Include = "Owner,LicenseNr,TypeOfVehicle,MakeAndModel,Color,Length,Weight,NrOfWheels" )] Vehicle vehicle )
        {
            if ( ModelState.IsValid && vehicle.TypeOfVehicle != VehicleType.None ) {

                vehicle.LicenseNr = vehicle.LicenseNr.Trim().ToUpper();

                if ( db.Vehicles.Where( v => v.LicenseNr == vehicle.LicenseNr ).FirstOrDefault() != null ) {
                    ViewBag.UserFailMessage = "An other vehicle have the same registation number!";
                }
                else {
                    vehicle.TimeParked = DateTime.Now;
                    vehicle.Parked = false;
                    ViewBag.UserFailMessage = Garage.ParkVehicle( vehicle, db );

                    db.Vehicles.Add( vehicle );
                    db.SaveChanges();
                    return RedirectToAction( "Index" );
                }
            }

            return View( vehicle );
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit( int? id )
        {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Vehicle vehicle = db.Vehicles.Find( id );
            if ( vehicle == null ) {
                return HttpNotFound();
            }
            return View( vehicle );
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "Id,Owner,LicenseNr,TypeOfVehicle,Length,Weight,Parked" )] Vehicle chgVehicle )
        {
            if ( ModelState.IsValid ) {
                Vehicle oldVehicle = db.Vehicles.Find( chgVehicle.Id );
                var entry = db.Entry( oldVehicle );
                entry.State = EntityState.Detached;

                entry = db.Entry( chgVehicle );
                entry.State = EntityState.Modified;

                if ( chgVehicle.Parked && !oldVehicle.Parked )
                    ViewBag.UserFailMessage = Garage.ParkVehicle( chgVehicle, db);
                else
                    entry.Property( "TimeParked" ).IsModified = false;

                db.SaveChanges();

                if ( !chgVehicle.Parked && oldVehicle.Parked )
                    return RedirectToAction( "Receipt", new { id = chgVehicle.Id } );
                return RedirectToAction( "Index" );
            }
            return View( chgVehicle );
        }

        public ActionResult TogglePark( int? id )
        {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            Vehicle vehicle = db.Vehicles.Find( id );
            bool wasParked = vehicle.Parked;

            if (!vehicle.Parked) {
                ViewBag.UserFailMessage = Garage.ParkVehicle( vehicle, db);
            }
            else {
                vehicle.Parked = false;
            }

            db.SaveChanges();

            if ( wasParked )
                return Content( "/Vehicles/Receipt/" + id );
            return PartialView( "_ParkedTableData", vehicle );
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete( int? id )
        {
            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Vehicle vehicle = db.Vehicles.Find( id );
            if ( vehicle == null ) {
                return HttpNotFound();
            }
            return View( vehicle );
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int id )
        {
            Vehicle vehicle = db.Vehicles.Find( id );
            db.Vehicles.Remove( vehicle );
            db.SaveChanges();
            return RedirectToAction( "Index" );
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
