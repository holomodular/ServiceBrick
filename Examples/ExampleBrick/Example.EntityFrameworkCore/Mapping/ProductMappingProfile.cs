using AutoMapper;
using Example.Api;

namespace Example.EntityFrameworkCore
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.ID, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.ProductCategories, y => y.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.ID))
                .ForMember(x => x.Categories, y => y.MapFrom(z =>
                    z.ProductCategories.Select(x => x.Category).ToList()));
        }
    }
}