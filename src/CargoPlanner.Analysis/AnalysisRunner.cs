using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CargoPlanner.Algos;
using CargoPlanner.Models;

namespace CargoPlanner.Analysis
{
    class AnalysisRunner
    {
        public static void AnalysisScenarios()
        {
            // Container sizes
            var width = 200;
            var height = 300;
            var depth = 500;
            // Set instance sizes
            var instanceSizes = new int[] { 250, 500, 750, 1000, 2000 };
            //var instanceSizes = new int[] { 250, 500 };
            // Get all the sorts
            var itemsOrders = Enum.GetValues(typeof(ItemsOrder)).Cast<ItemsOrder>().ToList();
            // Get the algorithm
            var algorithm = new BestFitAlgo();
            var path = "results.csv";
            using var w = new StreamWriter(path);
            // Write a header
            w.WriteLine($@"instance_size;items_order;load_constraint;3D;trucks_used;calculation_time;average_container_volume_utilization;worst_container_volume_utilization;average_axle_load");
            var instance = InstanceGenerator.Generate(itemsAmount: instanceSizes[0], width: width,
               height: height, depth: depth);
            foreach (var instanceSize in instanceSizes)
            {
                instance = InstanceGenerator.GenerateMore(instance, instanceSize);
                foreach (var itemsOrder in itemsOrders)
                {
                    Console.WriteLine($@"Calculating instance_size={instance.Items.Count}, items_order={itemsOrder.ToString()}");
                    //var instanceCopyLoad = new Instance(instance);
                    var instanceCopyNoLoad = new Instance(instance);
                    var instanceCopy2DLoad = new Instance(instance);
                    var instanceCopy2DNoLoad = new Instance(instance);
                    instanceCopy2DLoad.Truck.FrontAxle.MaximumLoad /= 2;
                    instanceCopy2DLoad.Truck.RearAxle.MaximumLoad /= 2;
                    //var resultWithLoad = algorithm.Solve(instanceCopyLoad, itemsOrder, loadConstraint: true);
                    var resultWithoutLoad = algorithm.Solve(instanceCopyNoLoad, itemsOrder, loadConstraint: false, threeDimensional: true);
                    var result2DLoad = algorithm.Solve(instanceCopy2DLoad, itemsOrder, loadConstraint: true, threeDimensional: false);
                    var result2DNoLoad = algorithm.Solve(instanceCopy2DNoLoad, itemsOrder, loadConstraint: false, threeDimensional: false);
                    //var metricsWithLoad = AlgorithmResultMetrics.FromAlgorithmResult(resultWithLoad);
                    var metricsWithoutLoad = AlgorithmResultMetrics.FromAlgorithmResult(resultWithoutLoad);
                    var metrics2DLoad = AlgorithmResultMetrics.FromAlgorithmResult(result2DLoad);
                    var metrics2DNoLoad = AlgorithmResultMetrics.FromAlgorithmResult(result2DNoLoad);
                    //w.WriteLine($@"{instanceCopyLoad.Items.Count};{itemsOrder.ToString()};true;{metricsWithLoad.ContainersUsed};{metricsWithLoad.CalculationTime.TotalSeconds};{metricsWithLoad.AverageContainerVolumeUtilization};{metricsWithLoad.WorstUtilizationExceptLastOne};{metricsWithLoad.AverageAxleLoadExceptLastOne}");
                    w.WriteLine($@"{instanceCopyNoLoad.Items.Count};{itemsOrder.ToString()};false;true;{metricsWithoutLoad.ContainersUsed};{metricsWithoutLoad.CalculationTime.TotalSeconds};{metricsWithoutLoad.AverageContainerVolumeUtilization};{metricsWithoutLoad.WorstUtilizationExceptLastOne};{metricsWithoutLoad.AverageAxleLoadExceptLastOne}");
                    w.WriteLine($@"{instance.Items.Count};{itemsOrder.ToString()};true;false;{metrics2DLoad.ContainersUsed};{metrics2DLoad.CalculationTime.TotalSeconds};{metrics2DLoad.AverageContainerVolumeUtilization};{metrics2DLoad.WorstUtilizationExceptLastOne};{metrics2DLoad.AverageAxleLoadExceptLastOne}");
                    w.WriteLine($@"{instance.Items.Count};{itemsOrder.ToString()};false;false;{metrics2DNoLoad.ContainersUsed};{metrics2DNoLoad.CalculationTime.TotalSeconds};{metrics2DNoLoad.AverageContainerVolumeUtilization};{metrics2DNoLoad.WorstUtilizationExceptLastOne};{metrics2DNoLoad.AverageAxleLoadExceptLastOne}");
                }
            }
        }
        static void Main(string[] args)
        {
            AnalysisScenarios();
        }
    }
}
