using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Interfaces;

namespace Bank.Issuer.Services.Calculator.Master
{
    public class Master:ICalculator
    {

        private readonly IRepository<Origin> _originRepository;

        public Master(IRepository<Origin> originRepository)
        {
            _originRepository = originRepository;
        }
        public async Task<decimal> Calculate(decimal amount)
        {
           var originFromDb=await _originRepository.GetByConditionAsync(x => x.Name.ToUpper() == Library.Enums.Origin.MASTER.ToString());
           var origin = originFromDb.FirstOrDefault();
           if (origin != null)  return amount / 100 * origin.Rate;
           return 0;
        }
    }
}
