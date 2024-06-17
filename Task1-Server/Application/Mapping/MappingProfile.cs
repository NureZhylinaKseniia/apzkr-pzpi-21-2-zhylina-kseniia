using AutoMapper;
using Application.Models;
using Application.ViewModels;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginView>().ReverseMap();
            CreateMap<Manager, ManagerView>().ReverseMap();
            CreateMap<Booking, BookingView>().ReverseMap();
            CreateMap<Workplace, WorkplaceView>().ReverseMap();
            CreateMap<Room, RoomView>().ReverseMap();
            CreateMap<RoomView, RoomParamView>().ReverseMap();
            CreateMap<CoworkingSpace, CoworkingSpaceView>().ReverseMap();
        }
    }
}