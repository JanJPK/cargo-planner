using NUnit.Framework;

namespace CargoPlanner.Models.Tests
{
    public class ItemTests
    {
        private const int width = 10;
        private const int height = 20;

        private const int depth = 30;

        private const double weight = 10;

        private static Item CreateDummyItem()
        {
            return new Item(0, width, height, depth, weight);
        }

        [Test]
        public void TestItemRotationZ()
        {
            var item = CreateDummyItem();

            item.Rotate(Rotation.Z);

            Assert.AreEqual(height, item.Width);
            Assert.AreEqual(width, item.Height);
        }

        [Test]
        public void TestItemRotationY()
        {
            var item = CreateDummyItem();

            item.Rotate(Rotation.Y);

            Assert.AreEqual(width, item.Depth);
            Assert.AreEqual(depth, item.Width);
        }

        [Test]
        public void TestItemRotationXY()
        {
            var item = CreateDummyItem();

            item.Rotate(Rotation.XY);

            Assert.AreEqual(width, item.Depth);
            Assert.AreEqual(height, item.Width);
            Assert.AreEqual(depth, item.Height);
        }

        [Test]
        public void TestItemRotationX()
        {
            var item = CreateDummyItem();

            item.Rotate(Rotation.X);

            Assert.AreEqual(height, item.Depth);
            Assert.AreEqual(depth, item.Height);
        }

        [Test]
        public void TestItemRotationXZ()
        {
            var item = CreateDummyItem();

            item.Rotate(Rotation.XZ);

            Assert.AreEqual(width, item.Height);
            Assert.AreEqual(height, item.Depth);
            Assert.AreEqual(depth, item.Width);
        }
    }
}