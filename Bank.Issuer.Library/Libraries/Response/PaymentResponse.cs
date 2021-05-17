using Bank.Issuer.Library.Libraries.Base;

namespace Bank.Issuer.Library.Libraries.Response
{
    public class PaymentResponse:BaseResponse
    {
        public string TransactionId { get; set; }
    }
}
