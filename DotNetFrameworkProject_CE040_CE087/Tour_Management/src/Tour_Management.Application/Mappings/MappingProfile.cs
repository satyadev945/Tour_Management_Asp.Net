using AutoMapper;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>()
            .ForMember(destination => destination.CreatedDate, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.IsActive, options => options.MapFrom(_ => true));
        CreateMap<TourUpdateDto, Tour>()
            .ForMember(destination => destination.ModifiedDate, options => options.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Booking, BookingDto>()
            .ForMember(destination => destination.TourName, options => options.MapFrom(source => source.Tour != null ? source.Tour.Name : string.Empty));
        CreateMap<BookingCreateDto, Booking>()
            .ForMember(destination => destination.BookingDate, options => options.MapFrom(_ => DateTime.UtcNow));

        CreateMap<ApplicationUserProfile, UserProfileDto>();
        CreateMap<UserProfileCreateDto, ApplicationUserProfile>()
            .ForMember(destination => destination.CreatedDate, options => options.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UserProfileUpdateDto, ApplicationUserProfile>();
    }
}
