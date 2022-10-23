using AutoMapper;
using Example.Api;

namespace Example.MongoDb
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Categories, y => y.Ignore());
        }
    }
}