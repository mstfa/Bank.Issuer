using System;
using System.Threading.Tasks;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries.Base;
using Bank.Issuer.Library.Models;

namespace Bank.Issuer.Services
{
    public class AccountService:IAccountService
    {
        public Task<AccountModel> GetAccountBalanceAsync(long accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
