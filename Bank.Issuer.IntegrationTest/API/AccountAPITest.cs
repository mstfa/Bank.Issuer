using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alba;
using Bank.Issuer.API;
using Bank.Issuer.Data;
using Bank.Issuer.Library.Libraries.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Bank.Issuer.IntegrationTest.API
{
    public class AccountAPITest
    {
        public AccountAPITest()
        {
            var options = new DbContextOptionsBuilder<BankIssuerDbContext>()
                .UseInMemoryDatabase(databaseName: "BankIssuer")
                .Options;
            using var context = new BankIssuerDbContext(options);
            context.Database.EnsureCreated();
            SeedDb.Initialize(context);
        }

        [Fact]
        public async Task should_query_single_account_balance()
        {
            using var system = SystemUnderTest.ForStartup<Startup>();
            var request = new BaseRequest()
            {
                AccountId = "4755"
            };

            var result = await system.PostJson(request, "/account")
                .Receive<BaseResponse>();

            Assert.Equal( 1001.88, result.Balance);
        }
    }
}
