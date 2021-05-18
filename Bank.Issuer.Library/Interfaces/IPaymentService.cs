using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Libraries;
using Bank.Issuer.Library.Libraries.Base;

namespace Bank.Issuer.Library.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentResponse> PaymentActionAsync(BaseRequest paymentRequest);
    }
}
