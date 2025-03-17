using AutoMapper;
using SWP391_CareSkin_BE.DTOS.Requests.Momo;
using SWP391_CareSkin_BE.DTOS.Responses.Momo;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class MomoMapper : Profile
    {
        public MomoMapper()
        {
            // Mapping from DTO -> Model
            CreateMap<MomoPaymentRequestDto, MomoPayment>()
                .ForMember(dest => dest.MomoPaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PaymentTime, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            // Mapping from Callback DTO -> Model
            CreateMap<MomoCallbackDto, MomoCallback>()
                .ForMember(dest => dest.MomoCallbackId, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
                
            // Mapping from Model -> Status DTO
            CreateMap<MomoPayment, MomoPaymentStatusDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid))
                .ForMember(dest => dest.PaymentTime, opt => opt.MapFrom(src => src.PaymentTime));
        }
    }
}
