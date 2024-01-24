using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class InsuranceRuleConstants
    {
        public static List<InsuranceRule> Ranges => new()
            {
                new InsuranceRule()
                {
                    MaxSalesPrice = 500,
                    InsurancePrice = 0
                },
                new InsuranceRule()
                {
                    MaxSalesPrice = 2000,
                    MinSalesPrice = 500,
                    InsurancePrice = 1000
                },
                new InsuranceRule()
                {
                    MinSalesPrice = 2000,
                    InsurancePrice = 2000
                }
            };
    }
}
