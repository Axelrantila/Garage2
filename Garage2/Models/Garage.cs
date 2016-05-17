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

            AdjacentLotSequences = new [] {
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

        static public int NrOfParkingLots { get;  }
        static public int MinLotNr { get; }
        static public int MaxLotNr { get; }
        static public Tuple<int,int>[] AdjacentLotSequences { get; }
        static public Dictionary<VehicleType,float> NrOfLotsRequired { get; }

        public static bool IsAdjacent( int lot, int nrAdjacentNeeded )
        {
            foreach (var seq in AdjacentLotSequences) {
                if (seq.Item1 <= lot && lot <= seq.Item2) {
                    int endLot = lot + nrAdjacentNeeded - 1;
                    if ( seq.Item1 <= endLot && endLot <= seq.Item2 )
                        return true;
                }
            }
            return false;
        }
    }
}
