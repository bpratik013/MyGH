﻿using AutoMapper;
using GigHub.DTO;
using GigHub.Models;

namespace GigHub.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Gig, GigDto>();
            Mapper.CreateMap<Notification, NotificationDto>();

            //using automapper for mappig source and target type
        }
    }
}