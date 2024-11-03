using AutoMapper;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CartService.Domain.Entities;

namespace OnlineShopping.CartService.Application.Common.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Item, ItemDTO>();
    }
}