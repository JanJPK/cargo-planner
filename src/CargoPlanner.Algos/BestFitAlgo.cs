using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using CargoPlanner.Models;

namespace CargoPlanner.Algos
{
    public enum ItemsOrder
    {
        VolumeAsc = 0,
        VolumeDesc = 1,
        WeightAsc = 2,
        WeightDesc = 4,
        Random = 8
    }

    public enum PackingDirection
    {
        ByWidth = 0,
        ByHeight = 1,
        ByDepth = 2
    }

    public class BestFitAlgo : IAlgo
    {
        public AlgoResult Solve(Instance instance, ItemsOrder order, bool loadConstraint = false, bool threeDimensional = true)
        {
            var items = instance.Items;

            // Start the algorithm
            // Measure time
            var watch = Stopwatch.StartNew();

            // Sort the items accordingly to "order" argument
            items = SortItems(items, order);

            // Choose packing direction
            var packingDirection =
                ChoosePackingDirection(instance.Truck.Width, instance.Truck.Height, instance.Truck.Depth);
            // Create necessary lists
            var notPackedItems = new List<Item>(items);
            var resultTrucks = new List<Truck>();

            while (notPackedItems.Count > 0)
            {
                var itemsToPack = new List<Item>(notPackedItems);
                notPackedItems.Clear();
                // Create a new container
                var currentTruck = CreateTruck(instance.Truck);
                // Fill itemsToPack[0] item at (0, 0, 0) position
                currentTruck.FitInitialItem(itemsToPack.First());

                foreach (var currentItem in itemsToPack.Skip(1))
                {
                    var fitted = false;

                    for (var p = 0; p < 3; p++)
                    {
                        var k = 0;
                        while (k < currentTruck.Items.Count && !fitted)
                        {
                            var containerItem = currentTruck.Items[k];
                            var pivotNumber = ChoosePivotNumber(packingDirection, p);
                            var pivot = ChoosePivot(containerItem, pivotNumber, threeDimensional: threeDimensional);

                            fitted = currentTruck.TryToPackItem(currentItem, pivot, loadConstraint);

                            k++;
                        }
                    }

                    if (!fitted) notPackedItems.Add(currentItem);
                }

                resultTrucks.Add(currentTruck);
            }
            watch.Stop();
            return new AlgoResult
            {
                Trucks = resultTrucks,
                Time = watch.Elapsed
            };
        }

        public static List<Item> SortItems(List<Item> items, ItemsOrder order)
        {
            switch (order)
            {
                case ItemsOrder.VolumeAsc:
                {
                    return items.OrderBy(x => x.Volume).ToList();
                }
                case ItemsOrder.VolumeDesc:
                {
                    return items.OrderByDescending(x => x.Volume).ToList();
                }
                case ItemsOrder.WeightAsc:
                {
                    return items.OrderBy(x => x.Weight).ToList();
                }
                case ItemsOrder.WeightDesc:
                {
                    return items.OrderByDescending(x => x.Weight).ToList();
                }
                case ItemsOrder.Random:
                {
                    return items.OrderBy(x => Guid.NewGuid()).ToList();
                }
                default:
                {
                    throw new UnknownItemsSortMethodException(order);
                }
            }
        }

        public static PackingDirection ChoosePackingDirection(int width, int height, int depth)
        {
            if (width < height &&
                width < depth)
                return PackingDirection.ByWidth;
            if (depth < height &&
                depth < width)
                return PackingDirection.ByDepth;
            if (height < width &&
                height < depth)
                return PackingDirection.ByHeight;
            return PackingDirection.ByWidth;
        }

        public static int ChoosePivotNumber(PackingDirection packingDirection, int p)
        {
            switch (packingDirection)
            {
                case PackingDirection.ByWidth:
                {
                    return p;
                }
                case PackingDirection.ByHeight:
                {
                    switch (p)
                    {
                        case 0:
                        {
                            return 2;
                        }
                        case 1:
                        {
                            return 0;
                        }
                        case 2:
                        {
                            return 1;
                        }
                    }

                    break;
                }
                case PackingDirection.ByDepth:
                {
                    switch (p)
                    {
                        case 0:
                        {
                            return 1;
                        }
                        case 1:
                        {
                            return 0;
                        }
                        case 2:
                        {
                            return 2;
                        }
                    }

                    break;
                }
            }

            throw new UnableToFindPivotNumberException();
        }

        public static Point ChoosePivot(Item containerItem, int pivotNumber, bool threeDimensional)
        {
            switch (pivotNumber)
            {
                case 0:
                {
                    return new Point(containerItem.Position.X + containerItem.Width,
                        containerItem.Position.Y, containerItem.Position.Z);
                }
                case 1:
                {
                    return new Point(containerItem.Position.X, containerItem.Position.Y,
                        containerItem.Position.Z + containerItem.Depth);
                }
                case 2:
                {
                    var itemHeight = threeDimensional ? containerItem.Height : 0;
                    return new Point(containerItem.Position.X,
                        containerItem.Position.Y + itemHeight, containerItem.Position.Z);
                }
            }

            throw new UnableToFindPivotException();
        }

        private static Truck CreateTruck(Truck truck)
        {
            return new Truck(truck.FrontAxle, truck.RearAxle, truck.Width, truck.Height, truck.Depth, truck.ContainerOffset);
        }
    }
}