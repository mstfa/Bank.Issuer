using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Bank.Issuer.Services.Base
{
    public abstract class BaseService
    {
        protected IBankIssuerMapper BankIssuerMapper { get; set; }
        protected IMemoryCache BankIssuerMemoryCache { get; set; }
        protected BaseService(IServiceProvider provider)
        {
            BankIssuerMapper = provider.GetService<IBankIssuerMapper>();
            BankIssuerMemoryCache = provider.GetService<IMemoryCache>();
        }
    }
}
