using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Interfaces;

namespace Bank.Issuer.Services.Calculator.Visa
{
    public class Visa:ICalculator
    {
        private readonly IRepository<Origin> _originRepository;

        public Visa(IRepository<Origin> originRepository)
        {
            _originRepository = originRepository;
        }

        public async Task<decimal> Calculate(decimal amount)
        {
            var originFromDb = await _originRepository.GetByConditionAsync(x => x.Name.ToUpper() == Library.Enums.Origin.VISA.ToString());
            var origin = originFromDb.FirstOrDefault();
            if (origin != null) return amount / 100 * origin.Rate;
            return 0;
        }
    }
}
