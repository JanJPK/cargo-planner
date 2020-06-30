using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CargoPlanner.Algos;
using CargoPlanner.Models;

namespace CargoPlanner.Analysis
{
    public class AlgorithmResultMetrics
    {
        public int ContainersUsed { get; set; }
        public TimeSpan CalculationTime { get; set; }
        public double AverageContainerVolumeUtilization { get; set; }
        public double WorstUtilizationExceptLastOne { get; set; }
        public double AverageAxleLoadExceptLastOne { get; set; }

        public AlgorithmResultMetrics(int containersUsed, TimeSpan calculationTime, double averageContainerVolumeUtilization, double worstUtilizationExceptLastOne, double averageAxleLoadExceptLastOne)
        {
            ContainersUsed = containersUsed;
            CalculationTime = calculationTime;
            AverageContainerVolumeUtilization = averageContainerVolumeUtilization;
            WorstUtilizationExceptLastOne = worstUtilizationExceptLastOne;
            AverageAxleLoadExceptLastOne = averageAxleLoadExceptLastOne;
        }

        public static AlgorithmResultMetrics FromAlgorithmResult(AlgoResult result)
        {
            var containersUsed = result.Trucks.Count;
            var calculationTime = result.Time;

            var volumeUtilizations = result.Trucks.Select(CalculateContainerVolumeUtilization).ToList();
            var averageContainerVolumeUtilization =
                volumeUtilizations.Aggregate(0.0, (current, utilization) => current + Math.Pow(utilization, 2)) /
                containersUsed * 100;
            var worstUtilizationExceptLastOne = Math.Pow(volumeUtilizations.Count > 1? volumeUtilizations.Take(volumeUtilizations.Count - 1).Min(): volumeUtilizations.Min(), 2) * 100;
            var averageAxleLoadExceptLastOne = result.Trucks.Aggregate(0.0,
                                                   (current, truck) =>
                                                       current +
                                                       Math.Pow((truck.FrontAxle.FinalLoad / truck.FrontAxle.MaximumLoad + truck.RearAxle.FinalLoad / truck.RearAxle.MaximumLoad) / 2, 2)) /
                                               containersUsed * 100;
;           return new AlgorithmResultMetrics(containersUsed: containersUsed, calculationTime: calculationTime, averageContainerVolumeUtilization: averageContainerVolumeUtilization, worstUtilizationExceptLastOne: worstUtilizationExceptLastOne, averageAxleLoadExceptLastOne: averageAxleLoadExceptLastOne);
        }

        public static double CalculateContainerVolumeUtilization(Truck truck)
        {
            return truck.Items.Aggregate(0.0, (current, item) => current + item.Volume) / truck.Volume;
        }
    }
}

