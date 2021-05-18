using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Issuer.Library.Interfaces
{
    public interface IBankIssuerMapper
    {
        Task<TDestination> Map<TDestination>(object source);
        Task<TDestination> Map<TSource, TDestination>(TSource source);
    }
}
