using AutoMapper;
using Model = CargoPlanner.Models;
using Input = CargoPlanner.API.Dtos.Input;
using Result = CargoPlanner.API.Dtos.Result;
using ListItem = CargoPlanner.API.Dtos.ListItem;

namespace CargoPlanner.API.Mapping
{
    public class CargoPlannerMapperProfile : Profile
    {
        public CargoPlannerMapperProfile()
        {
            // Model <---> Input
            CreateMap<Input.Instance, Model.Instance>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Items, o => o.Ignore());

            CreateMap<Model.Instance, Input.Instance>()
                .ForMember(d => d.Items, o => o.Ignore());

            CreateMap<Input.Truck, Model.Truck>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Items, o => o.Ignore())
                .ForMember(d => d.ContainerOffset, o => o.MapFrom(s => 0))
                .ForMember(d => d.AllowedRotations, o => o.MapFrom(s => Model.Rotation.NONE));

            CreateMap<Model.Truck, Input.Truck>();

            CreateMap<Model.Axle, Input.Axle>();

            CreateMap<Input.Axle, Model.Axle>()
                .ForMember(d => d.FinalLoad, o => o.Ignore());

            CreateMap<Model.Item, Input.Item>()
                .ForMember(d => d.Count, o => o.Ignore());

            CreateMap<Input.Item, Model.Item>();

            CreateMap<Input.User, Model.User>()
                .ForMember(d => d.PasswordHash, o => o.MapFrom(s => s.Password));

            CreateMap<Model.User, Input.User>();

            // Model ----> Result
            var modelItemToResultItemConverter = new ModelItemToResultItemConverter();

            CreateMap<Model.Result, Result.Result>();
            
            CreateMap<Model.Truck, Result.Truck>()
                .ForMember(d => d.Mesh, o => o.MapFrom(s => new Result.Mesh
                {
                    X = s.Width / 2,
                    Y = s.Height / 2, 
                    Z = s.Depth / 2,
                    Color = "#424242",
                    Opacity = 0.2
                }));

            CreateMap<Model.Axle, Result.Axle>();

            CreateMap<Model.Item, Result.Item>()
                .ConvertUsing(modelItemToResultItemConverter);

            // Model ----> ListItem
            CreateMap<Model.Instance, ListItem.Instance>()
                .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Count))
                .ForMember(d => d.Display, o => o.MapFrom(s => $"{s.CreationDate:f}"));

            CreateMap<Model.Result, ListItem.Result>()
                .ForMember(d => d.Display, o => o.MapFrom(s => $"{s.CreationDate:f}"));
        }
    }
}