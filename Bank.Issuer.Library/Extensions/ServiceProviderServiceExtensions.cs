using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Issuer.Library.Extensions
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
