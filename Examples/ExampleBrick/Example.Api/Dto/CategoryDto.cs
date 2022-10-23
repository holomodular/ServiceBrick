using ServiceBrick;

namespace Example.Api
{
    public partial class CategoryDto : DataTransferObject
    {
        public CategoryDto()
        {
        }

        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
    }
}