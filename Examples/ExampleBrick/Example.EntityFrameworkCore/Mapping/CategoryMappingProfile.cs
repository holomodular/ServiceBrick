using AutoMapper;
using Example.Api;

namespace Example.EntityFrameworkCore
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.ID, y => y.MapFrom(z => !string.IsNullOrEmpty(z.StorageKey) ? int.Parse(z.StorageKey) : 0))
                .ForMember(x => x.ProductCategories, y => y.Ignore());

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.ID));
        }
    }
}