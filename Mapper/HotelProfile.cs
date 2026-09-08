using AutoMapper;
using HotelManagement.Dtos;
using HotelManagement.Entity;

namespace HotelManagement.Mapper
{
    public class HotelProfile :Profile
    {
        public HotelProfile() 
        {
            CreateMap<Hotel, HotelDto>();
            CreateMap<Hotel, HotelDto>();
        }
    }
    
 }

