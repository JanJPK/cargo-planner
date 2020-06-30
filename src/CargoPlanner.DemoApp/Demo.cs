using System;
using System.Linq;
using CargoPlanner.Algos;
using CargoPlanner.Models;

namespace CargoPlanner.DemoApp
{
    internal class Demo
    {
        private static void Main(string[] args)
        {
            var algorithm = new BestFitAlgo();
            var instance = InstanceGenerator.Generate(1000);
            var result = algorithm.Solve(instance, ItemsOrder.VolumeDesc, false);
            PrintResult(result);
            Console.ReadLine();
        }

        private static void PrintResult(AlgoResult algorithmResult)
        {
            foreach (var iterator in algorithmResult.Trucks.Select((Value, Index) => new {Value, Index}))
            {
                var truck = iterator.Value;
                Console.WriteLine($@"===========================Container {iterator.Index}===========================");
                Console.WriteLine(
                    $@"===========================Container volume: {truck.Volume}===========================");
                Console.WriteLine(
                    $@"===========================Container volume utilization: {AlgoResult.CalculateContainerVolumeUtilization(truck)}===========================");
                Console.WriteLine(
                    $@"===========================Steering axle load: {truck.FrontAxle.FinalLoad}===========================");
                Console.WriteLine(
                    $@"===========================Driving axle load: {truck.RearAxle.FinalLoad}===========================");

                foreach (var cargoItem in truck.Items) Console.WriteLine(cargoItem);
            }
        }
    }
}