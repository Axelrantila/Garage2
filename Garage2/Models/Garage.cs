using Garage2.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public static class Garage
    {
        static Garage()
        {
            NrOfParkingLots = 71;
            MinLotNr = 1;
            MaxLotNr = 71;

            AdjacentLotSequences = new[] {
                new Tuple<int, int>( 1, 10 ),
                new Tuple<int, int>( 11, 31 ),
                new Tuple<int, int>( 32, 35 ),
                new Tuple<int, int>( 39, 71 ),
            };

            NrOfLotsRequired = new Dictionary<VehicleType, float> {
                {VehicleType.None, 1 },
                {VehicleType.Car, 1 },
                {VehicleType.SUV, 1 },
                {VehicleType.Motorcycle, 0.33f },
                {VehicleType.CarTrailer, 2 },
                {VehicleType.Truck, 3 }
            };
        }

        static public int NrOfParkingLots { get; }
        static public int MinLotNr { get; }
        static public int MaxLotNr { get; }
        static public Tuple<int, int>[] AdjacentLotSequences { get; }
        static public Dictionary<VehicleType, float> NrOfLotsRequired { get; }

        public static bool IsAdjacent(int lot, int nrAdjacentNeeded)
        {
            if ( nrAdjacentNeeded <= 1 )
                return true;

            foreach (var seq in AdjacentLotSequences)
            {
                if (seq.Item1 <= lot && lot <= seq.Item2)
                {
                    int endLot = lot + nrAdjacentNeeded - 1;
                    if (seq.Item1 <= endLot && endLot <= seq.Item2)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Parks a vehicle in the garage, returns an error message if failing, otherwise null.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>Returns an error message if failing, otherwise null.</returns>
        public static string ParkVehicle(Vehicle vehicle, VehicleContext db)
        {
            var parkedVehicles = db.Vehicles.Where(v => v.Parked).OrderBy(v => v.ParkingLot).ToList();

            //int lot = TryParkSmallVehicle(vehicle, parkedVehicles);
            int lot = -1;

            if (lot < 0)
                lot = TryParkVehicle(vehicle, parkedVehicles);

            if (lot < 0)
            {
                vehicle.Parked = false;
                return "No space for the vehicle in the garage";
            }

            vehicle.Parked = true;
            vehicle.ParkingLot = lot;
            vehicle.TimeParked = DateTime.Now;

            return null;
        }

        static private int NrOfIntLotsNeeded(Vehicle vehicle)
        {
            return 1;
            return (int)Math.Ceiling(Garage.NrOfLotsRequired[vehicle.TypeOfVehicle] - 0.00001f);
        }

        static private int TryParkVehicle(Vehicle vehicle, List<Vehicle> parkedVehicles)
        {
            int nrLotsNeeded = NrOfIntLotsNeeded(vehicle);

            int candidateLot = Garage.MinLotNr;
            for (int i = 0; i < parkedVehicles.Count; i++)
            {
                if (parkedVehicles[i].ParkingLot > candidateLot)
                {
                    int freeLots = parkedVehicles[i].ParkingLot - candidateLot;
                    if (freeLots >= nrLotsNeeded && Garage.IsAdjacent(candidateLot, (int)nrLotsNeeded))
                    {
                        return candidateLot;
                    }
                }
                candidateLot = parkedVehicles[i].ParkingLot + NrOfIntLotsNeeded(parkedVehicles[i]);
            }

            do
            {
                if (Garage.IsAdjacent(candidateLot, (int)nrLotsNeeded))
                    return candidateLot;
                candidateLot++;
            } while (candidateLot <= Garage.MaxLotNr);

            return int.MinValue;
        }

        static int TryParkSmallVehicle(Vehicle vehicle, List<Vehicle> parkedVehicles)
        {
            int candidateLot = int.MinValue;

            float nrLotsNeeded = Garage.NrOfLotsRequired[vehicle.TypeOfVehicle];
            if (nrLotsNeeded >= 1)
                return candidateLot;

            for (int i = 0; i < parkedVehicles.Count; i++)
            {
                if (Garage.NrOfLotsRequired[parkedVehicles[i].TypeOfVehicle] <= 0.99f)
                {
                    float freeSpace = (float)1;
                    int j = i;
                    for (; j < parkedVehicles.Count && parkedVehicles[j].ParkingLot == parkedVehicles[i].ParkingLot; j++)
                        freeSpace -= Garage.NrOfLotsRequired[parkedVehicles[j].TypeOfVehicle];
                    if (nrLotsNeeded <= freeSpace + 0.00001)
                    {
                        candidateLot = parkedVehicles[i].ParkingLot;
                        break;
                    }
                    i = j - 1;
                }
            }

            return candidateLot;
        }
    }
}
