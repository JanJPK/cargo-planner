namespace CargoPlanner.Models
{
    public class Dimensions
    {
        public Dimensions(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
    }
}