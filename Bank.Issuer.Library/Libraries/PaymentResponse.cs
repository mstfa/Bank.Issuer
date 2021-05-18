using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Libraries.Base;

namespace Bank.Issuer.Library.Libraries
{
    public class PaymentResponse:BaseResponse
    {
        public string TransactionId { get; set; }
    }
}
