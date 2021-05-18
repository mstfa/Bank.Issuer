using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Enums;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries;
using Bank.Issuer.Services.Base;

namespace Bank.Issuer.Services
{
    public class CalculationService: BaseService, ICalculationService
    {
        private readonly Func<Origin, ICalculator> _calculatorDelegate;

        public CalculationService(IServiceProvider provider,
            Func<Origin, ICalculator> calculatorDelegate
        ) : base(provider)
        {
            _calculatorDelegate = calculatorDelegate;
        }

        public async  Task<decimal> CalculateCommissionByOrigin(Origin origin, decimal amount)
        {
            ICalculator calculator = origin switch
            {
                Origin.MASTER => _calculatorDelegate(Origin.MASTER),
                Origin.VISA => _calculatorDelegate(Origin.VISA),
                _ => null
            };
            if (calculator != null) return await calculator?.Calculate(amount);

            return 0;
        }
    }
}
