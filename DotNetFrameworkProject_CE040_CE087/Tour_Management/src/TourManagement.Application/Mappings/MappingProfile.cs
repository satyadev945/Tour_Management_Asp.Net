using AutoMapper;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
        CreateMap<TourUpdateDto, Tour>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour != null ? src.Tour.Name : string.Empty));
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
        CreateMap<BookingUpdateDto, Booking>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<ApplicationUser, ApplicationUserDto>();
    }
}
