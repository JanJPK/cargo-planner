using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CargoPlanner.Models
{
    public class Item
    {
        public Item()
        {
            
        }

        public Item(int type, 
                    int width,
                    int height,
                    int depth,
                    double weight,
                    Point position = new Point())
        {
            Type = type;
            Width = width;
            Height = height;
            Depth = depth;
            Volume = Width * Height * Depth;
            Weight = weight;
            Position = position;
        }

        public Item(Item item)
        {
            Type = item.Type;
            Description = item.Description;
            Width = item.Width;
            Height = item.Height;
            Depth = item.Depth;
            Volume = Width * Height * Depth;
            Weight = item.Weight;
            Position = item.Position;
        }


        public int Type { get; set; }
        
        public string Description { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public int Volume { get; set; }

        public double Weight { get; set; }

        public Point Position { get; set; }

        public Rotation AllowedRotations { get; set; }

        public void Rotate(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Z:
                    (Height, Width) = (Width, Height);
                    break;
                case Rotation.Y:
                    (Width, Depth) = (Depth, Width);
                    break;
                case Rotation.XY:
                    (Depth, Height) = (Height, Depth);
                    (Width, Depth) = (Depth, Width);
                    break;
                case Rotation.X:
                    (Depth, Height) = (Height, Depth);
                    break;
                case Rotation.XZ:
                    (Depth, Height) = (Height, Depth);
                    (Height, Width) = (Width, Height);
                    break;
                case Rotation.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public void RotateBack(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Z:
                    (Height, Width) = (Width, Height);
                    break;
                case Rotation.Y:
                    (Width, Depth) = (Depth, Width);
                    break;
                case Rotation.XY:
                    (Width, Depth) = (Depth, Width);
                    (Depth, Height) = (Height, Depth);
                    break;
                case Rotation.X:
                    (Depth, Height) = (Height, Depth);
                    break;
                case Rotation.XZ:
                    (Height, Width) = (Width, Height);
                    (Depth, Height) = (Height, Depth);
                    break;
                case Rotation.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        public override string ToString()
        {
            return string.Format(
                $@"Item({Type}, w={Width}, h={Height}, d={Depth}, v={Volume}, weight={Weight}, pos={Position})");
        }
    }
}