using System.Collections.Generic;
using Insurance.Api.Services.Insurance.Models;

namespace Insurance.Api.Constants
{
    public class InsuranceRuleConstants
    {
        public static List<ProductInsuranceRule> ProductInsuranceRules => new()
        {
            new ProductInsuranceRule()
            {
                MaxSalesPrice = 500,
                InsurancePrice = 0
            },
            new ProductInsuranceRule()
            {
                MaxSalesPrice = 2000,
                MinSalesPrice = 500,
                InsurancePrice = 1000
            },
            new ProductInsuranceRule()
            {
                MinSalesPrice = 2000,
                InsurancePrice = 2000
            }
        };

        public static Dictionary<FrequentlyLostProductType, double> CartInsuranceRules => new()
        {
            {FrequentlyLostProductType.DigitalCamera, 500} 
        };

        public static double SpecialTypeInsuranceCost => 500;
    }
}
