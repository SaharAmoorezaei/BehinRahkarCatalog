
using AutoMapper;
using BehinRahkar.Catalog.Application.Dto;

namespace BehinRahkar.Catalog.Application.AutoMapper
{
    public class DomaintoDtoMappingProfile : Profile
    {
        public DomaintoDtoMappingProfile()
        {
            CreateMap<Domain.Models.Product, ProductDto>()
                 .ReverseMap();
            CreateMap<Domain.Models.Product, ProductDtoV2>()
                 .ReverseMap();

        }
    }
}
