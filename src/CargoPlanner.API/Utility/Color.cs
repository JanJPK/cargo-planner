using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = CargoPlanner.API.Dtos.Result;

namespace CargoPlanner.API.Utility
{
    public static class Color
    {
        public static string GetColor()
        {
            var rgb = Color.GetRgbColor();
            return rgb.GetHexColor();
        }

        public static int[] GetRgbColor()
        {
            var rgb = new[] { 75, 75, 75 };
            var rng = new Random();

            var decision = rng.Next(3, 7);
            while (decision == 4)
            {
                decision = rng.Next(3, 7);
            }

            if ((decision & 1) == 1)
            {
                rgb[0] = rng.Next(90, 256);
            }

            if ((decision & 2) == 2)
            {
                rgb[1] = rng.Next(90, 256);
            }

            if ((decision & 4) == 4)
            {
                rgb[2] = rng.Next(90, 256);
            }

            return rgb;
        }

        public static string GetHexColor(this int[] rgb)
        {
            if (rgb.Length != 3)
            {
                return null;
            }

            if (rgb.Any(e => e > 255 || e < 0))
            {
                return null;
            }

            return $"#{Convert.ToString(rgb[0], 16)}{Convert.ToString(rgb[1], 16)}{Convert.ToString(rgb[2], 16)}";
        }
    }
}
