using System;

namespace CargoPlanner.API.Dtos.ListItem
{
    public class Instance
    {
        public Guid Id { get; set; }

        public int ItemCount { get; set; }

        public string Display { get; set; }
    }
}