using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Insurance.Api.Constants;
using Insurance.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
        [HttpPost]
        [Route("api/insurance/product")]
        public InsuranceDto CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            int productId = toInsure.ProductId;

            BusinessRules.GetProductType(ProductApi, productId, ref toInsure);
            BusinessRules.GetSalesPrice(ProductApi, productId, ref toInsure);

            if (!toInsure.ProductTypeHasInsurance)
            {
                toInsure.InsuranceValue = 0;
                return toInsure;
            }

            float insurance = 0f;
            foreach (var rule in InsuranceRuleConstants.Ranges) 
            {
                insurance += CalculateInsuranceRule(rule, toInsure.SalesPrice);
            };

            if (Enum.IsDefined(typeof(SpecialProductType), toInsure.ProductTypeName))
                insurance += 500;

            toInsure.InsuranceValue = insurance;

            return toInsure;
        }

        private float CalculateInsuranceRule(InsuranceRule rule, float salesPrice)
        {
            if (rule.MaxSalesPrice == null && rule.MinSalesPrice == null)
                return 0;

            if((rule.MaxSalesPrice == null || salesPrice < rule.MaxSalesPrice)
                && (rule.MinSalesPrice == null || salesPrice >= rule.MinSalesPrice))
            {
                return rule.InsurancePrice;
            }

            return 0;
        }

        public class InsuranceDto
        {
            public int ProductId { get; set; }
            public float InsuranceValue { get; set; }
            [JsonIgnore]
            public string ProductTypeName { get; set; }
            [JsonIgnore]
            public bool ProductTypeHasInsurance { get; set; }
            [JsonIgnore]
            public float SalesPrice { get; set; }
        }

        private const string ProductApi = "http://localhost:5002";
    }
}