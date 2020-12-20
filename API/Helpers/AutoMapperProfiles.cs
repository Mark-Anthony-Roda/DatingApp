using System;
using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>().ForMember(destination => destination.PhotoUrl, option => option.MapFrom(source => source.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(destination => destination.Age, options => options.MapFrom(source => source.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(destination => destination.SenderPhotoUrl, option => option.MapFrom(source => source.Sender.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(destination => destination.RecipientPhotoUrl, option => option.MapFrom(source => source.Recipient.Photos.FirstOrDefault(photo => photo.IsMain).Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(date => DateTime.SpecifyKind(date, DateTimeKind.Utc));
        }
    }
}