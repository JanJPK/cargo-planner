using System.Collections.Generic;
using System.Linq;
using CargoPlanner.API.Utility;
using NUnit.Framework;
using Input = CargoPlanner.API.Dtos.Input;
using Model = CargoPlanner.Models;

namespace CargoPlanner.API.Tests
{
    [TestFixture]
    public class ItemExtensionTests
    {
        [Test]
        public void Should_Copy_Properties_When_Packing()
        {
            // Arrange
            var unpackedItem = new Model.Item
            {
                Type = 1,
                Description = "test",
                Width = 100,
                Height = 200,
                Depth = 300,
                Weight = 400
            };
            var unpackedItems = new List<Model.Item> {unpackedItem};

            // Act
            var packedItem = unpackedItems.Pack().FirstOrDefault();

            // Assert
            Assert.IsNotNull(packedItem);
            Assert.AreEqual(unpackedItem.Description, packedItem.Description);
            Assert.AreEqual(unpackedItem.Width, packedItem.Width);
            Assert.AreEqual(unpackedItem.Height, packedItem.Height);
            Assert.AreEqual(unpackedItem.Depth, packedItem.Depth);
            Assert.AreEqual(unpackedItem.Weight, packedItem.Weight);
        }

        [Test]
        public void Should_Copy_Properties_When_Unpacking()
        {
            // Arrange
            var packedItem = new Input.Item
            {
                Type = 1,
                Description = "test",
                Width = 100,
                Height = 200,
                Depth = 300,
                Weight = 400,
                Count = 1
            };
            var packedItems = new[] {packedItem};

            // Act
            var unpackedItem = packedItems.Unpack().FirstOrDefault();

            // Assert
            Assert.IsNotNull(unpackedItem);
            Assert.AreEqual(packedItem.Description, unpackedItem.Description);
            Assert.AreEqual(packedItem.Width, unpackedItem.Width);
            Assert.AreEqual(packedItem.Height, unpackedItem.Height);
            Assert.AreEqual(packedItem.Depth, unpackedItem.Depth);
            Assert.AreEqual(packedItem.Weight, unpackedItem.Weight);
        }

        [Test]
        public void Should_Pack_Items()
        {
            // Arrange
            var unpackedItems = new List<Model.Item>
            {
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 2
                },
                new Model.Item
                {
                    Type = 2
                }
            };
            var expectedPackedItems = new List<Input.Item>
            {
                new Input.Item
                {
                    Type = 1,
                    Count = 3
                },
                new Input.Item
                {
                    Type = 2,
                    Count = 2
                }
            };

            // Act
            var packedItems = unpackedItems.Pack();

            // Assert
            Assert.AreEqual(expectedPackedItems.Count, packedItems.Count);
            Assert.AreEqual(packedItems[0].Count, expectedPackedItems[0].Count);
            Assert.AreEqual(packedItems[1].Count, expectedPackedItems[1].Count);
            Assert.AreEqual(packedItems[0].Type, expectedPackedItems[0].Type);
            Assert.AreEqual(packedItems[1].Type, expectedPackedItems[1].Type);
        }

        [Test]
        public void Should_Unpack_Items()
        {
            // Arrange
            var packedItems = new[]
            {
                new Input.Item
                {
                    Type = 1,
                    Count = 3
                },
                new Input.Item
                {
                    Type = 2,
                    Count = 2
                }
            };
            var expectedUnpackedItems = new List<Model.Item>
            {
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 1
                },
                new Model.Item
                {
                    Type = 2
                },
                new Model.Item
                {
                    Type = 2
                }
            };

            // Act
            var unpackedItems = packedItems.Unpack();

            // Assert
            Assert.AreEqual(expectedUnpackedItems.Count, unpackedItems.Count);
            for (var i = 0; i < unpackedItems.Count; i++)
            {
                Assert.AreEqual(unpackedItems[i].Type, expectedUnpackedItems[i].Type);
            }
        }
    }
}