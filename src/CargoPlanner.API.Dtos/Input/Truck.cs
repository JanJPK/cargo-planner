namespace CargoPlanner.API.Dtos.Input
{
    public class Truck
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public Axle FrontAxle { get; set; }

        public Axle RearAxle { get; set; }
    }
}