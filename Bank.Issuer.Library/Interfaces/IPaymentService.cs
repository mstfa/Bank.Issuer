using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Libraries.Request;
using Bank.Issuer.Library.Libraries.Response;

namespace Bank.Issuer.Library.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentResponse> PaymentActionAsync(PaymentRequest paymentRequest);
    }
}
