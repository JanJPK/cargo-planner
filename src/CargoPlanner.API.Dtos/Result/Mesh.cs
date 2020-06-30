using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CargoPlanner.Models;

namespace CargoPlanner.API.Dtos.Result
{
    public class Mesh
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public string Color { get; set; }

        public double Opacity { get; set; }
    }
}
