using System;

namespace CargoPlanner.Models
{
    [Flags]
    public enum Rotation
    {
        NONE = 0,
        Z = 1,
        Y = 2,
        XY = 4,
        X = 8,
        XZ = 16,
        ALL = Z | X | Y | XY | X | XZ
    }
}