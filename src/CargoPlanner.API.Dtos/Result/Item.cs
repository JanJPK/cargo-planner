using CargoPlanner.Models;

namespace CargoPlanner.API.Dtos.Result
{
    public class Item
    {
        public int Type { get; set; }

        public string Description { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public double Weight { get; set; }

        public Mesh Mesh { get; set; }
    }
}       