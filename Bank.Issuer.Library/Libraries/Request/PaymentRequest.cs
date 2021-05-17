using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Libraries.Base;

namespace Bank.Issuer.Library.Libraries.Request
{
    public class PaymentRequest:BaseRequest
    {
        public string MessageType { get; set; }
        public string TransactionId { get; set; }
        public string Origin { get; set; }
        public decimal Amount { get; set; }
    }
}
