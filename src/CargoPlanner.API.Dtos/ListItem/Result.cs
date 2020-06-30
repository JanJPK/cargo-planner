using System;

namespace CargoPlanner.API.Dtos.ListItem
{
    public class Result
    {
        public Guid Id { get; set; }

        public Guid InstanceId { get; set; }

        public string Display { get; set; }
    }
}