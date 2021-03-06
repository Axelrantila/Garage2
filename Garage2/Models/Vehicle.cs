﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public enum VehicleType
    {
        None,
        Car,
        SUV,
        Motorcycle,
        CarTrailer,
        Truck
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string LicenseNr { get; set; }

        public int TypeOfVehicleNewId { get; set; }
        public virtual TypeOfVehicle TypeOfVehicleNew { get; set; }

        public VehicleType TypeOfVehicle { get; set; }
        public int? MemberId { get; set; }
        public virtual Member Member { get; set; }
        public string MakeAndModel { get; set; }
        public string Color { get; set; }
        public float Length { get; set; }
        public float Weight { get; set; }
        public int NrOfWheels { get; set; }
        public DateTime TimeParked { get; set; }
        public bool Parked { get; set; }
        public int ParkingLot { get; set; }

        public int HoursParked()
        {
            return (int)((DateTime.Now - TimeParked).TotalHours + 1);
        }
    }
}