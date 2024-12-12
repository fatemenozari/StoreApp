using AutoMapper;
using OnlineStore.Api.Services.DTO;
using OnlineStore.Core.Domain;

namespace OnlineStore.Services;

public class MappingProfile  : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
         
        CreateMap<Category, CategoryDto>(); 
    }  
}