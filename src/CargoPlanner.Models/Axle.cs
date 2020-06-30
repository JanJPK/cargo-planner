using Microsoft.EntityFrameworkCore;

namespace CargoPlanner.Models
{
    [Owned]
    public class Axle
    {
        public Axle()
        {
            
        }

        public Axle(int offset, double initialLoad, double maximumLoad)
        {
            Offset = offset;
            InitialLoad = initialLoad;
            FinalLoad = initialLoad;
            MaximumLoad = maximumLoad;
        }

        public Axle(Axle axle)
        {
            Offset = axle.Offset;
            InitialLoad = axle.InitialLoad;
            FinalLoad = axle.InitialLoad;
            MaximumLoad = axle.MaximumLoad;
        }

        // Offset from front of truck
        public int Offset { get; set; }

        public double InitialLoad { get; set; }

        public double FinalLoad { get; set; }

        public double MaximumLoad { get; set; }
    }
}