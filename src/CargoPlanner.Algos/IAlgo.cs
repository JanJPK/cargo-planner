using CargoPlanner.Models;

namespace CargoPlanner.Algos
{
    public interface IAlgo
    {
        AlgoResult Solve(Instance instance, ItemsOrder order, bool loadConstraint = false, bool threeDimensional = true);
    }
}