using AutoMapper;
using Example.Api;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.PartitionKey, y => y.MapFrom<PartitionKeyResolver>())
                .ForMember(x => x.RowKey, y => y.MapFrom<RowKeyResolver>())
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore());

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.PartitionKey));
        }
    }
}