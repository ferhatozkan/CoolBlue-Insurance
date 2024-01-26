using Insurance.Api.Models.Dto;

namespace Insurance.Api.Services.Chain
{
    public class InsuranceChainService : IInsuranceChainService
    {
        private readonly CanBeInsuredHandler _canBeInsuredHandler;
        private readonly PriceRuleInsuranceHandler _priceRuleInsuranceHandler;
        private readonly SpecialTypeInsuranceHandler _specialTypeInsurance;
        private readonly SurchargeRateHandler _surchargeRateHandler;

        public InsuranceChainService(
            CanBeInsuredHandler canBeInsuredHandler, 
            PriceRuleInsuranceHandler priceRuleInsuranceHandler, 
            SpecialTypeInsuranceHandler specialTypeInsurance, 
            SurchargeRateHandler surchargeRateHandler)
        {
            _canBeInsuredHandler = canBeInsuredHandler;
            _priceRuleInsuranceHandler = priceRuleInsuranceHandler;
            _specialTypeInsurance = specialTypeInsurance;
            _surchargeRateHandler = surchargeRateHandler;

            _canBeInsuredHandler.SetNext(_priceRuleInsuranceHandler);
            _priceRuleInsuranceHandler.SetNext(_specialTypeInsurance);
            _specialTypeInsurance.SetNext(_surchargeRateHandler);
        }

        public ProductInsuranceChainDto Handle(ProductInsuranceChainDto productInsuranceDto)
        {
            return _canBeInsuredHandler.Handle(productInsuranceDto);
        }
    }
}
