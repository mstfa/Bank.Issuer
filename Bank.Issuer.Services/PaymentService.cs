using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries.Request;
using Bank.Issuer.Library.Libraries.Response;

namespace Bank.Issuer.Services
{
    public class PaymentService:IPaymentService
    {
        public Task<PaymentResponse> PaymentActionAsync(PaymentRequest paymentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
