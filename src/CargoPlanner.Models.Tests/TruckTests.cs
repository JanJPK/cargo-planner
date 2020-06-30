using System.Linq;
using NUnit.Framework;

namespace CargoPlanner.Models.Tests
{
    public class TruckTests
    {
        private const int itemWidth = 5;
        private const int itemHeight = 10;

        private const int itemDepth = 15;

        private const double itemWeight = 5;

        private const int containerWidth = 20;
        private const int containerHeight = 30;
        private const int containerDepth = 50;
        private const int containerOffset = 20;

        private static Item CreateDummyItem(int width = itemWidth, int height = itemHeight, int depth = itemDepth,
            double weight = itemWeight, Point position = new Point())
        {
            return new Item(0, width, height, depth, weight, position);
        }

        private static Truck CreateDummyTruck()
        {
            var frontAxle = new Axle(0, 5000, 20000);
            var rearAxle = new Axle(500, 8000, 30000);
            return new Truck(frontAxle, rearAxle, containerWidth,
                containerHeight, containerDepth, containerOffset);
        }

        private static object[] ItemsToCheck()
        {
            return new object[]
            {
                new object[] // Intersecting on all axes
                {
                    CreateDummyItem(5, 5, 5, 5, new Point(0, 0, 0)),
                    CreateDummyItem(5, 5, 5, 5, new Point(2, 2, 2)),
                    true
                },
                new object[] // Intersecting on x axis
                {
                    CreateDummyItem(5, 5, 5, 5, new Point(0, 0, 0)),
                    CreateDummyItem(5, 5, 5, 5, new Point(2, 10, 10)),
                    false
                },
                new object[] // Intersecting on y axis
                {
                    CreateDummyItem(5, 5, 5, 5, new Point(0, 0, 0)),
                    CreateDummyItem(5, 5, 5, 5, new Point(10, 2, 10)),
                    false
                },
                new object[] // Intersecting on z axis
                {
                    CreateDummyItem(5, 5, 5, 5, new Point(0, 0, 0)),
                    CreateDummyItem(5, 5, 5, 5, new Point(10, 10, 2)),
                    false
                },
                new object[] // Intersecting on no axes
                {
                    CreateDummyItem(5, 5, 5, 5, new Point(0, 0, 0)),
                    CreateDummyItem(5, 5, 5, 5, new Point(10, 10, 10)),
                    false
                }
            };
        }

        [Test]
        public void TestFitInitialItem()
        {
            var item = CreateDummyItem();
            var truck = CreateDummyTruck();

            truck.FitInitialItem(item);

            Assert.AreEqual(1, truck.Items.Count);
            Assert.AreEqual(truck.Items.First().Position, new Point(0, 0, 0));
            Assert.AreEqual(truck.Items.First(), item);
        }

        [TestCase(0, 0, 0, false)]
        [TestCase(10, 0, 0, true)]
        [TestCase(0, 10, 0, true)]
        [TestCase(0, 0, 10, true)]
        [TestCase(10, 10, 10, true)]
        public void TestItemFits(int x, int y, int z, bool fits)
        {
            var truck = CreateDummyTruck();
            var item = CreateDummyItem();

            truck.FitInitialItem(CreateDummyItem(10, 10, 10));

            Assert.AreEqual(fits, truck.ItemFits(item, new Point(x, y, z)));
        }

        [TestCaseSource(nameof(ItemsToCheck))]
        public void TestItemIntersectsWithItem(Item item1, Item item2, bool expected)
        {
            var intersects = Truck.ItemIntersectsWithItem(item1, item2);

            Assert.AreEqual(expected, intersects);
        }
    }
}