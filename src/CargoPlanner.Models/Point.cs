using System;

namespace CargoPlanner.Models
{
    public struct Point : IEquatable<Point>
    {
        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override string ToString()
        {
            return string.Format($@"Point({X}, {Y}, {Z})");
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (ReferenceEquals(p1, p2)) return true;

            if (ReferenceEquals(p1, null)) return false;
            if (ReferenceEquals(p2, null)) return false;
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return X.Equals(other.X)
                   && Y.Equals(other.Y)
                   && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}