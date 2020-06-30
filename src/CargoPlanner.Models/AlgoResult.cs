using System;
using System.Collections.Generic;
using System.Linq;

namespace CargoPlanner.Models
{
    public class AlgoResult
    {
        public List<Truck> Trucks { get; set; }

        public TimeSpan Time { get; set; }

        public static double CalculateContainerVolumeUtilization(Truck truck)
        {
            return truck.Items.Sum(x => x.Volume) / (double) truck.Volume;
        }
    }
}