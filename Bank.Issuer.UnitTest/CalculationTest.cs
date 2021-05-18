using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Issuer.Core;
using Bank.Issuer.Data;
using Bank.Issuer.Library.Enums;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Services;
using Bank.Issuer.Services.Calculator.Master;
using Bank.Issuer.Services.Calculator.Visa;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bank.Issuer.UnitTest
{
    public class DependencySetupFixture
    {
        public DependencySetupFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<BankIssuerDbContext>(options => options.UseInMemoryDatabase("BankIssuer"));

            var options = new DbContextOptionsBuilder<BankIssuerDbContext>()
                .UseInMemoryDatabase(databaseName: "BankIssuer")
                .Options;
            using var context = new BankIssuerDbContext(options);
            context.Database.EnsureCreated();
            SeedDb.Initialize(context);

            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            serviceCollection.AddTransient<Master>();
            serviceCollection.AddTransient<Visa>();

            serviceCollection.AddTransient<Func<Origin, ICalculator>>(serviceProvider => key =>
            {
                return key switch
                {
                    Origin.MASTER => serviceProvider.GetService<Master>(),
                    Origin.VISA => serviceProvider.GetService<Visa>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            serviceCollection.AddTransient<ICalculationService, CalculationService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }

    public class CalculationTest : IClassFixture<DependencySetupFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public CalculationTest(DependencySetupFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }


        [Fact]
        public async Task calculate_rate()
        {
            using var scope = _serviceProvider.CreateScope();
            ICalculationService _calculationService = scope.ServiceProvider.GetService<ICalculationService>();
            var masterRateAmount = await _calculationService.CalculateCommissionByOrigin(Origin.MASTER, 100);

            Assert.Equal(2, masterRateAmount);

            var visaRateAmount = await _calculationService.CalculateCommissionByOrigin(Origin.VISA, 100);

            Assert.Equal(1, visaRateAmount);
        }
    }
}
