using AppointmentService.Application.Appointments;
using AppointmentService.Domain;
using AppointmentService.Application.Masters;
using AppointmentService.Application.Services;
using AppointmentService.Application.TimeSlots;

namespace AppointmentService.Application.Helpers;

public class MappingProfiles : AutoMapper.Profile
{
    public MappingProfiles()
    {
        CreateMap<Master, MasterDto>()
            .ForMember(d => d.Image, o => o.MapFrom(s => s.User.Image))
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName));
        CreateMap<AppointmentDto, Appointment>().ReverseMap();
        CreateMap<Service, ServiceDto>().ReverseMap();
        CreateMap<TimeSlotDto, TimeSlot>().ReverseMap();
    }
}