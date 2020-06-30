namespace CargoPlanner.API.Dtos.Result
{
    public class Axle
    {
        // Offset from front of truck
        public int Offset { get; set; }

        public double FinalLoad { get; set; }

        public double MaximumLoad { get; set; }
    }
}