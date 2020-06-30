using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Input = CargoPlanner.API.Dtos.Input;
using Result = CargoPlanner.API.Dtos.Result;
using Model = CargoPlanner.Models;

namespace CargoPlanner.API.Utility
{
    public static class ItemExtensions
    {
        public static List<Model.Item> Unpack(this IEnumerable<Input.Item> items)
        {
            var unpackedItems = new List<Model.Item>();

            foreach (var item in items)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    unpackedItems.Add(new Model.Item
                    {
                        Type = item.Type,
                        Description = item.Description,
                        Width = item.Width,
                        Height = item.Height,
                        Depth = item.Depth,
                        Weight = item.Weight
                    });
                }
            }

            return unpackedItems;
        }

        public static List<Input.Item> Pack(this IEnumerable<Model.Item> items)
        {
            var packedItems = new List<Input.Item>();

            foreach (var item in items)
            {
                var existingItem = packedItems.SingleOrDefault(i => i.Type == item.Type);
                if (existingItem != null)
                {
                    existingItem.Count++;
                }
                else
                {
                    packedItems.Add(new Input.Item
                    {
                        Type = item.Type,
                        Description = item.Description,
                        Width = item.Width,
                        Height = item.Height,
                        Depth = item.Depth,
                        Weight = item.Weight,
                        Count = 1
                    });
                }
            }

            return packedItems;
        }
    }
}
