using System;
using System.Collections.Generic;

namespace CargoPlanner.Models
{
    /// <summary>
    ///     Coupling of input data that can be easily persisted in database
    /// </summary>
    public class Instance
    {
        public Instance()
        {
            CreationDate = DateTime.Now;
            Items = new List<Item>();
            Truck = new Truck();
        }

        public Instance(List<Item> items,
                        Truck truck)
        {
            Id = Guid.NewGuid();
            UserId = Guid.Empty;
            CreationDate = DateTime.Now;
            Items = new List<Item>(items);
            Truck = new Truck(truck);
        }

        public Instance(Instance instance)
        {
            Id = instance.Id;
            UserId = instance.UserId;
            CreationDate = instance.CreationDate;
            Items = new List<Item>(instance.Items);
            Truck = new Truck(instance.Truck);
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreationDate { get; set; }

        public List<Item> Items { get; set; }

        public Truck Truck { get; set; }
    }
}