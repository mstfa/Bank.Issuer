using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Issuer.Library.Interfaces
{
    public interface ICalculator
    {
        Task<decimal> Calculate(decimal amount);
    }
}
