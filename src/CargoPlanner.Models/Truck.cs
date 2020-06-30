using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CargoPlanner.Models
{
    public class Truck
    {
        public Truck()
        {
            Items = new List<Item>();
            FrontAxle = new Axle();
            RearAxle = new Axle();
        }

        public Truck(Axle frontAxle, Axle rearAxle, int width, int height, int depth, int containerOffset)
        {
            Items = new List<Item>();
            FrontAxle = new Axle(frontAxle);
            RearAxle = new Axle(rearAxle);
            Width = width;
            Height = height;
            Depth = depth;
            Volume = Width * Height * Depth;
            ContainerOffset = containerOffset;
            AllowedRotations = Rotation.NONE
                             | Rotation.Z
                             | Rotation.Y
                             | Rotation.XY
                             | Rotation.X
                             | Rotation.XZ;
        }

        public Truck(Truck truck)
        {
            Items = new List<Item>(truck.Items);
            FrontAxle = new Axle(truck.FrontAxle);
            RearAxle = new Axle(truck.RearAxle);
            Width = truck.Width;
            Height = truck.Height;
            Depth = truck.Depth;
            Volume = truck.Volume;
            ContainerOffset = truck.ContainerOffset;
            AllowedRotations = truck.AllowedRotations;
        }

        public int Id { get; set; }

        public Rotation AllowedRotations { get; set; }

        public List<Item> Items { get; set; }

        public Axle FrontAxle { get; set; }

        public Axle RearAxle { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public int Volume { get; set; }

        public int ContainerOffset { get; set; }

        public void FitInitialItem(Item cargoItem)
        {
            var initialPosition = new Point(0, 0, 0);
            cargoItem.Position = initialPosition;
            TryToPackItem(cargoItem, initialPosition, loadConstraint: true);
        }

        public bool TryToPackItem(Item cargoItem, Point position, bool loadConstraint)
        {
            foreach(var rotation in RotationExtensions.PossibleRotations)
            {
                if (AllowedRotations.HasFlag(rotation))
                {
                    cargoItem.Rotate(rotation);
                    if (ItemFits(cargoItem, position))
                    {
                        if (!loadConstraint)
                        {
                            PackItem(cargoItem, position);
                            return true;
                        }
                        var axisOverloadData = ItemDoesNotOverloadAnyAxle(cargoItem, position);
                        if (axisOverloadData.doesNotOverload)
                        {
                            PackItem(cargoItem, position);
                            FrontAxle.FinalLoad += axisOverloadData.steeringAxleLoad;
                            RearAxle.FinalLoad += axisOverloadData.drivingAxleLoad;
                            return true;
                        }
                    }
                    cargoItem.RotateBack(rotation);
                }
            }

            return false;
        }

        public bool ItemFits(Item cargoItem, Point position)
        {
            var itemCopy = new Item(cargoItem) {Position = position};
            return ItemFitsInContainer(itemCopy) && !ItemIntersectsWithOther(itemCopy) && ItemDoesntHangInTheAir(itemCopy);
        }
        
        public (double steeringAxleLoad, double drivingAxleLoad, bool doesNotOverload) ItemDoesNotOverloadAnyAxle(
            Item cargoItem, Point position)
        {
            var itemCopy = new Item(cargoItem) {Position = position};
            var wheelbase = RearAxle.Offset - FrontAxle.Offset;
            var currentSteeringAxleLoad = 0.0;
            var currentDrivingAxleLoad = 0.0;

            currentSteeringAxleLoad += CalculateSteeringAxleLoad(itemCopy, wheelbase);
            currentDrivingAxleLoad += CalculateDrivingAxleLoad(itemCopy, wheelbase);

            var doesNotOverload = !(currentSteeringAxleLoad + FrontAxle.FinalLoad > FrontAxle.MaximumLoad) &&
                                  !(currentDrivingAxleLoad + RearAxle.FinalLoad > RearAxle.MaximumLoad);
            return (currentSteeringAxleLoad, currentDrivingAxleLoad, doesNotOverload);
        }

        public double CalculateSteeringAxleLoad(Item cargoItem, double wheelbase)
        {
            var cg = cargoItem.Position.Z + cargoItem.Depth / 2 + ContainerOffset;
            var weight = cargoItem.Weight;
            var steeringAxleToCg = cg - FrontAxle.Offset;

            return weight * (wheelbase - steeringAxleToCg) / wheelbase;
        }

        public double CalculateDrivingAxleLoad(Item cargoItem, double wheelbase)
        {
            var cg = cargoItem.Position.Z + cargoItem.Depth / 2 + ContainerOffset;
            var weight = cargoItem.Weight;
            var drivingAxleToCg = RearAxle.Offset - cg;

            return weight * (wheelbase - drivingAxleToCg) / wheelbase;
        }

        public void PackItem(Item cargoItem, Point position)
        {
            cargoItem.Position = position;
            Items.Add(cargoItem);
        }

        private bool ItemFitsInContainer(Item cargoItem)
        {
            if (cargoItem.Position.X + cargoItem.Width > Width) return false;

            if (cargoItem.Position.Y + cargoItem.Height > Height) return false;

            return cargoItem.Position.Z + cargoItem.Depth <= Depth;
        }

        private bool ItemIntersectsWithOther(Item cargoItem)
        {
            return Items.Any(packedItem => ItemIntersectsWithItem(cargoItem, packedItem));
        }

        public static bool ItemsIntersectOnXAxis(Item cargoItem1, Item cargoItem2)
        {
            return cargoItem1.Position.X < cargoItem2.Position.X + cargoItem2.Width &&
                   cargoItem1.Position.X + cargoItem1.Width > cargoItem2.Position.X;
        }

        public static bool ItemsIntersectOnYAxis(Item cargoItem1, Item cargoItem2)
        {
            return cargoItem1.Position.Y < cargoItem2.Position.Y + cargoItem2.Height &&
                   cargoItem1.Position.Y + cargoItem1.Height > cargoItem2.Position.Y;
        }

        public static bool ItemsIntersectOnZAxis(Item cargoItem1, Item cargoItem2)
        {
            return cargoItem1.Position.Z < cargoItem2.Position.Z + cargoItem2.Depth &&
                   cargoItem1.Position.Z + cargoItem1.Depth > cargoItem2.Position.Z;
        }

        public static bool ItemIntersectsWithItem(Item cargoItem1, Item cargoItem2)
        {
            if (cargoItem1.Position == cargoItem2.Position) return true;

            var xAxisIntersection = ItemsIntersectOnXAxis(cargoItem1, cargoItem2);
            var yAxisIntersection = ItemsIntersectOnYAxis(cargoItem1, cargoItem2);
            var zAxisIntersection = ItemsIntersectOnZAxis(cargoItem1, cargoItem2);

            return xAxisIntersection && yAxisIntersection && zAxisIntersection;
        }
        public bool ItemDoesntHangInTheAir(Item cargoItem)
        {
            return Items.Count == 0 || Items.Any(packedItem => ItemHasGround(cargoItem, packedItem));
        }
        public static bool ItemHasGround(Item cargoItem1, Item cargoItem2)
        {
            if (cargoItem1.Position.Y == 0) return true;

            if (cargoItem1.Position.Y != cargoItem2.Position.Y + cargoItem2.Height) return false;

            var xAxisIntersection = ItemsIntersectOnXAxis(cargoItem1, cargoItem2);
            var zAxisIntersection = ItemsIntersectOnZAxis(cargoItem1, cargoItem2);

            return xAxisIntersection && zAxisIntersection;
        }
    }
}