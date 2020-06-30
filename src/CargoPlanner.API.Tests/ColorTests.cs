using CargoPlanner.API.Utility;
using NUnit.Framework;

namespace CargoPlanner.API.Tests
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void Should_Convert_RGB_To_Hex()
        {
            // Arrange
            int[] rgb = {25, 125, 225};

            // Act
            var hex = rgb.GetHexColor();

            // Assert
            Assert.IsNotNull(hex);
            Assert.AreEqual("#197DE1", hex.ToUpper());
        }

        [Test]
        public void Should_Ignore_Arrays_Larger_Than_3()
        {
            // Arrange
            var rgb = new int[4];

            // Act
            var hex = rgb.GetHexColor();

            // Assert
            Assert.IsNull(hex);
        }

        [Test]
        public void Should_Ignore_Arrays_Smaller_Than_3()
        {
            // Arrange
            var rgb = new int[2];

            // Act
            var hex = rgb.GetHexColor();

            // Assert
            Assert.IsNull(hex);
        }

        [Test]
        public void Should_Ignore_Values_Larger_Than_255()
        {
            // Arrange
            int[] rgb = {256, 0, 0};

            // Act
            var hex = rgb.GetHexColor();

            // Assert
            Assert.IsNull(hex);
        }

        [Test]
        public void Should_Ignore_Values_Smaller_Than_0()
        {
            // Arrange
            int[] rgb = {-1, 0, 0};

            // Act
            var hex = rgb.GetHexColor();

            // Assert
            Assert.IsNull(hex);
        }
    }
}