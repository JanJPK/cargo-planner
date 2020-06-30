using AutoMapper;
using CargoPlanner.API.Utility;
using Model = CargoPlanner.Models;
using Result = CargoPlanner.API.Dtos.Result;

namespace CargoPlanner.API.Mapping
{
    public class ModelItemToResultItemConverter : ITypeConverter<Model.Item, Result.Item>
    {
        public Result.Item Convert(Model.Item source, Result.Item destination, ResolutionContext context)
        {
            return new Result.Item
            {
                Type = source.Type,
                Description = source.Description,
                Width = source.Width,
                Height = source.Height,
                Depth = source.Depth,
                Weight = source.Weight,
                Mesh = new Result.Mesh
                {
                    X = source.Position.X + source.Width / 2,
                    Y = source.Position.Y + source.Height / 2,
                    Z = source.Position.Z + source.Depth / 2,
                    Color = Color.GetColor(),
                    Opacity = 1.0
                }
            };
        }
    }
}
