using System;
using System.Collections.Generic;

namespace CargoPlanner.Models
{
    public class Result
    {
        public Result()
        {
            CreationDate = DateTime.Now;
        }

        public Result(Instance instance, AlgoResult result)
        {
            Id = Guid.NewGuid();
            UserId = instance.UserId;
            InstanceId = instance.Id;
            CreationDate = DateTime.Now;
            Trucks = new List<Truck>(result.Trucks);
            CalculationTimeInMs = result.Time.TotalMilliseconds;
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid InstanceId { get; set; }

        public DateTime CreationDate { get; set; }

        public List<Truck> Trucks { get; set; }

        public double CalculationTimeInMs { get; set; }
    }
}
