using System;
using System.Collections.Generic;

namespace CargoPlanner.API.Dtos.Input
{
    public class Instance
    {
        public Guid UserId { get; set; }

        public List<Item> Items { get; set; }

        public Truck Truck { get; set; }
    }
}