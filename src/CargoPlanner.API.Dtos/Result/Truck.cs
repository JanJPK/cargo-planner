namespace CargoPlanner.API.Dtos.Result
{
    public class Truck
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public Item[] Items { get; set; }

        public Mesh Mesh { get; set; }
    }
}