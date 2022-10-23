using Example.Api;
using Microsoft.Extensions.Logging;
using ServiceBrick;

namespace Example
{
    public partial class CategoryRule : BusinessRule
    {
        protected readonly ILogger _logger;

        public CategoryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CategoryRule>();
            Priority = PRIORITY_NORMAL;
        }

        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(CategoryDto),
                typeof(CategoryRule));
        }

        public override Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                if (context.Object is CategoryDto dto)
                {
                    if (string.IsNullOrEmpty(dto.Name))
                    {
                        response.AddMessage(
                            ResponseMessage.CreateError("Name is a required property"));
                        return Task.FromResult<IResponse>(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return Task.FromResult<IResponse>(response);
        }
    }
}