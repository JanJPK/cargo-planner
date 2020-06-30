using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoPlanner.Models
{
    public static class RotationExtensions
    {
        public static readonly Rotation[] PossibleRotations = new[]
            {Rotation.NONE, Rotation.Z, Rotation.Y, Rotation.XY, Rotation.X, Rotation.XZ};

        public static Rotation SetFlag(this Rotation a, Rotation b)
        {
            return a | b;
        }

        public static Rotation RemoveFlag(this Rotation a, Rotation b)
        {
            return a & (~b);
        }

        public static bool HasFlag(this Rotation a, Rotation b)
        {
            return (a & b) == b;
        }

        public static Rotation[] GetAllFlags()
        {
            return (Rotation[]) Enum.GetValues(typeof(Rotation));
        }
    }
}