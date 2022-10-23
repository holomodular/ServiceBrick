using AutoMapper;
using Example.Api;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.PartitionKey, y => y.MapFrom<PartitionKeyResolver>())
                .ForMember(x => x.RowKey, y => y.MapFrom<RowKeyResolver>())
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.PartitionKey))
                .ForMember(x => x.Categories, y => y.Ignore());
        }
    }
}