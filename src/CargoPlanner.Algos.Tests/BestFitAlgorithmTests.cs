using CargoPlanner.Models;
using NUnit.Framework;

namespace CargoPlanner.Algos.Tests
{
    public class BestFitAlgorithmTests
    {
        private static object[] ItemsToPivotOn()
        {
            return new object[]
            {
                new object[]
                {
                    CreateDummyItem(5, 5, 5),
                    0,
                    new Point(55, 5, 5)
                },
                new object[]
                {
                    CreateDummyItem(5, 5, 5),
                    1,
                    new Point(5, 5, 55)
                },
                new object[]
                {
                    CreateDummyItem(5, 5, 5),
                    2,
                    new Point(5, 55, 5)
                }
            };
        }

        private static Item CreateDummyItem(int x, int y, int z)
        {
            return new Item(0, 50, 50, 50, 100, new Point(x, y, z));
        }

        // Container dimensions
        [TestCase(50, 200, 300, PackingDirection.ByWidth)]
        [TestCase(100, 200, 50, PackingDirection.ByDepth)]
        [TestCase(100, 50, 300, PackingDirection.ByHeight)]
        [TestCase(50, 50, 50, PackingDirection.ByWidth)]
        public void TestChoosePackingDirection(int containerWidth, int containerHeight, int containerDepth,
            PackingDirection expected)
        {
            var packingDirection = BestFitAlgo.ChoosePackingDirection(containerWidth, containerHeight, containerDepth);

            Assert.AreEqual(expected, packingDirection);
        }

        [TestCase(PackingDirection.ByWidth, 0, 0)]
        [TestCase(PackingDirection.ByHeight, 0, 2)]
        [TestCase(PackingDirection.ByHeight, 1, 0)]
        [TestCase(PackingDirection.ByHeight, 2, 1)]
        [TestCase(PackingDirection.ByDepth, 0, 1)]
        [TestCase(PackingDirection.ByDepth, 1, 0)]
        [TestCase(PackingDirection.ByDepth, 2, 2)]
        public void TestChoosePivotNumber(PackingDirection packingDirection, int p, int expected)
        {
            var pivotNumber = BestFitAlgo.ChoosePivotNumber(packingDirection, p);

            Assert.AreEqual(expected, pivotNumber);
        }

        [TestCaseSource(nameof(ItemsToPivotOn))]
        public void TestChoosePivot(Item item, int pivotNumber, Point expected)
        {
            var pivot = BestFitAlgo.ChoosePivot(item, pivotNumber, true);

            Assert.AreEqual(expected, pivot);
        }
    }
}