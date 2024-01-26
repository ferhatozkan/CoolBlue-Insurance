using Insurance.Api.Application.Models.Dto;
using Insurance.Api.Application.Services.Insurance.Rules;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public class InsuranceChainService : IInsuranceChainService
    {

        private readonly IHandler _head;

        public InsuranceChainService(
            CanBeInsuredHandler canBeInsuredHandler,
            PriceRuleInsuranceHandler priceRuleInsuranceHandler,
            SpecialTypeInsuranceHandler specialTypeInsurance,
            SurchargeRateHandler surchargeRateHandler)
        {
            canBeInsuredHandler.SetNext(priceRuleInsuranceHandler);
            priceRuleInsuranceHandler.SetNext(specialTypeInsurance);
            specialTypeInsurance.SetNext(surchargeRateHandler);

            _head = canBeInsuredHandler;
        }

        public InsuranceDto Handle(InsuranceDto insuranceDto)
        {
            return _head.Handle(insuranceDto);
        }
    }
}