using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Enums;

namespace Bank.Issuer.Library.Interfaces
{
    public interface ICalculationService
    {
        public Task<decimal> CalculateCommissionByOrigin(Origin origin,decimal amount);
    }
}
