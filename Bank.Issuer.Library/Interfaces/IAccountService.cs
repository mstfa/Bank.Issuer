using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Libraries.Base;
using Bank.Issuer.Library.Libraries.Request;
using Bank.Issuer.Library.Libraries.Response;
using Bank.Issuer.Library.Models;

namespace Bank.Issuer.Library.Interfaces
{
    public interface IAccountService
    {
        public Task<AccountModel> GetAccountBalanceAsync(long accountNumber);

    }
}
