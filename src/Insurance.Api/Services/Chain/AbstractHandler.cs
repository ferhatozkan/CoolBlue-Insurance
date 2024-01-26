using Insurance.Api.Models.Dto;

namespace Insurance.Api.Services.Chain
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        public abstract ProductInsuranceChainDto Handle(ProductInsuranceChainDto insuranceDto);

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        protected ProductInsuranceChainDto NextChain(ProductInsuranceChainDto productInsuranceDto)
        {
            if (_nextHandler == null)
            {
                return productInsuranceDto;
            }

            return _nextHandler.Handle(productInsuranceDto);
        }
    }
}
