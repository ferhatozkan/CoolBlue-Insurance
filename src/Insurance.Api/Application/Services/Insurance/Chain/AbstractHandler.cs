using Insurance.Api.Application.Models.Dto;

namespace Insurance.Api.Application.Services.Insurance.Chain
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        public abstract InsuranceDto Handle(InsuranceDto insuranceDto);

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        protected InsuranceDto NextChain(InsuranceDto insuranceDto)
        {
            if (_nextHandler == null)
            {
                return insuranceDto;
            }

            return _nextHandler.Handle(insuranceDto);
        }
    }
}
