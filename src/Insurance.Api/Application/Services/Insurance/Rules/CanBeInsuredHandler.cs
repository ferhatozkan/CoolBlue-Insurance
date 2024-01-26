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

        public override InsuranceDto Handle(InsuranceDto insuranceDto)
        {
            var productTypeDto = _productApiClient.GetProductType(insuranceDto.ProductTypeId).Result;

            if (!productTypeDto.CanBeInsured)
            {
                insuranceDto.InsuranceCost = 0;

                _logger.LogInformation($"This product {insuranceDto.ProductId} with type {insuranceDto.ProductTypeId} cannot be insured!");

                return insuranceDto;
            }

            return NextChain(insuranceDto);
        }
    }
}
