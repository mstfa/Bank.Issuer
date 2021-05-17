using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Enums;
using Bank.Issuer.Library.Interfaces;

namespace Bank.Issuer.Services
{
    public class CalculationService:ICalculationService
    {
        public Task<decimal> CalculateCommissionByOrigin(Origin origin, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
