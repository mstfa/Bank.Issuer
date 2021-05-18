using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alba;
using Bank.Issuer.API;
using Bank.Issuer.Data;
using Bank.Issuer.Library.Enums;
using Bank.Issuer.Library.Libraries;
using Bank.Issuer.Library.Libraries.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bank.Issuer.IntegrationTest.API
{
    public class PaymentAPITest
    {
        public PaymentAPITest()
        {
            var options = new DbContextOptionsBuilder<BankIssuerDbContext>()
                .UseInMemoryDatabase(databaseName: "BankIssuer")
                .Options;
            using var context = new BankIssuerDbContext(options);
            context.Database.EnsureCreated();
            SeedDb.Initialize(context);
        }

        [Fact]
        public async Task should_post_payment_account()
        {
            using var system = SystemUnderTest.ForStartup<Startup>();
           
            var request = new BaseRequest()
            {
                AccountId = "4755",
                MessageType = MessageType.PAYMENT.ToString(),
                Origin = "VISA",
                Amount = "100"
            };

            var result = await system.PostJson(request, "/payment")
                .Receive<BaseResponse>();

            Assert.Equal(900.88, result.Balance);
        }

        [Fact]
        public async Task should_post_adjusment_account()
        {
            using var system = SystemUnderTest.ForStartup<Startup>();

            var requestPayment = new BaseRequest()
            {
                AccountId = "4755",
                MessageType = MessageType.PAYMENT.ToString(),
                Origin = Origin.VISA.ToString(),
                Amount = "100"
            };

            var resultPayment = await system.PostJson(requestPayment, "/payment")
                .Receive<PaymentResponse>();

            Assert.Equal(900.88, resultPayment.Balance);

            var transactionId = resultPayment.TransactionId;
            var requestAdjustment = new BaseRequest()
            {
                AccountId = "4755",
                MessageType = MessageType.ADJUSTMENT.ToString(),
                Origin = Origin.VISA.ToString(),
                Amount = "100",
                TransactionId = transactionId
            };

            var resultAdjustment = await system.PostJson(requestAdjustment, "/payment")
                .Receive<PaymentResponse>();

            Assert.Equal(1001.88, resultAdjustment.Balance);
        }
    }
}
