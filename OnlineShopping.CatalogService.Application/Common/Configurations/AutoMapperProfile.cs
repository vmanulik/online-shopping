using AutoMapper;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;

namespace OnlineShopping.CatalogService.Application.Common.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Category, CategoryDTO>();
        CreateMap<Product, ProductDTO>();
    }
}