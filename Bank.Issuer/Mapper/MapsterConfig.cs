using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Models;
using FastExpressionCompiler;
using Mapster;

namespace Bank.Issuer.API.Mapper
{
    public class MapsterConfig
    {
        public static TypeAdapterConfig GetConfiguredMappingConfig()
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<Account, AccountModel>().TwoWays();
            config.NewConfig<Transaction, TransactionModel>().TwoWays();

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetEntryAssembly());
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
            config.Default.PreserveReference(true);
            config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

            return config;
        }
    }
}
