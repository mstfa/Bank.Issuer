using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries.Base;
using Bank.Issuer.Library.Models;
using Bank.Issuer.Services.Base;

namespace Bank.Issuer.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IRepository<Account> accountRepository, IServiceProvider provider
        ) : base(provider)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountModel> GetAccountBalanceAsync(long accountNumber)
        {
            var account = await _accountRepository.GetByConditionAsync(x => x.AccountNumber == accountNumber);

            var accountModel = await BankIssuerMapper.Map<AccountModel>(account.FirstOrDefault());


            return accountModel;
        }
    }
}
