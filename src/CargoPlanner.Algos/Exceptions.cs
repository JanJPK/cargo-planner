using System;
using CargoPlanner.Models;

namespace CargoPlanner.Algos
{
    internal class UnableToFindPivotNumberException : Exception
    {
        public UnableToFindPivotNumberException() : base("Couldn't find pivot number!")
        {
        }
    }

    internal class UnableToFindPivotException : Exception
    {
        public UnableToFindPivotException() : base("Couldn't find pivot!")
        {
        }
    }

    internal class UnknownItemsSortMethodException : Exception
    {
        public UnknownItemsSortMethodException(ItemsOrder order) : base(@$"Couldn't find the sorting method: {order}!")
        {
        }
    }
}