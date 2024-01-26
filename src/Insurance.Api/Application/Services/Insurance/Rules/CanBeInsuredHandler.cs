using Insurance.Api.Application.Clients;
using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Chain;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Application.Services.Insurance.Rules
{
    public class CanBeInsuredHandler : AbstractHandler
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ILogger<CanBeInsuredHandler> _logger;

        public CanBeInsuredHandler(IProductApiClient productApiClient, ILogger<CanBeInsuredHandler> logger)
        {
            _productApiClient = productApiClient;
            _logger = logger;
        }

        public override ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto)
        {
            var productTypeDto = _productApiClient.GetProductType(productInsuranceDto.ProductTypeId).Result;

            if (!productTypeDto.CanBeInsured)
            {
                productInsuranceDto.InsuranceCost = 0;

                _logger.LogInformation($"This product {productInsuranceDto.ProductId} with type {productInsuranceDto.ProductTypeId} cannot be insured!");

                return productInsuranceDto;
            }

            return NextChain(productInsuranceDto);
        }
    }
}
