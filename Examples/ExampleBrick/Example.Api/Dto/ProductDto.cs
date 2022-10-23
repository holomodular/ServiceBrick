using ServiceBrick;

namespace Example.Api
{
    public partial class ProductDto : DataTransferObject
    {
        public ProductDto()
        {
        }

        public string Name { get; set; }
        public bool IsExpired { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}