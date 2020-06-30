using System;
using System.Collections.Generic;
using System.Linq;
using CargoPlanner.Models;

namespace CargoPlanner.Algos
{
    public class InstanceGenerator
    {
        public static Instance Generate(int itemsAmount = 10, int width = 200, int height = 300, int depth = 500)
        {
            var items = GenerateItems(itemsAmount, width, height, depth);

            var frontAxle = new Axle(200, 5000, 20000);
            var rearAxle = new Axle(700, 8000, 30000);
            var containerOffset = 200;

            return new Instance(items, new Truck(frontAxle, rearAxle, width, height, depth, containerOffset));
        }

        public static List<Item> GenerateItems(int itemsToGenerate, int width, int height, int depth)
        {
            var generator = new Random();

            var maxWidth = (int)(width / 3.0);
            var maxHeight = (int)(height / 3.0);
            var maxDepth = (int)(depth / 3.0);
            var maxAmount = itemsToGenerate / 10.0 > 1 ? (int)(itemsToGenerate / 10.0) : 1;

            var itemsCount = 0;
            var itemNumber = 0;

            var items = new List<Item>();

            while (itemsCount < itemsToGenerate)
            {
                var randomAmount = generator.Next(1, Math.Min(maxAmount, itemsToGenerate - itemsCount));

                var randomWidth = generator.Next(1, maxWidth);
                var randomHeight = generator.Next(1, maxHeight);
                var randomDepth = generator.Next(1, maxDepth);
                var volume = randomWidth * randomHeight * randomDepth;

                var randomDensity = generator.NextDouble() * (1.5 - 1) + 1;
                var randomWeight = randomDensity * volume / 1000.0;
                for (var i = 0; i < randomAmount; i++)
                    items.Add(new Item(itemNumber, randomWidth, randomHeight, randomDepth, randomWeight));
                itemNumber++;
                itemsCount += randomAmount;
            }

            return items;
        }
      
        public static Instance GenerateMore(Instance instance, int finalItemsAmount)
        {
            var itemsToGenerate = finalItemsAmount - instance.Items.Count;
            if (itemsToGenerate <= 0)
            {
                return instance;
            }

            var generatedItems = GenerateItems(itemsToGenerate, instance.Truck.Width, instance.Truck.Height,
                instance.Truck.Depth);
            var items = instance.Items.Concat(generatedItems).ToList();
            return new Instance(items, instance.Truck);
        }
    }
}