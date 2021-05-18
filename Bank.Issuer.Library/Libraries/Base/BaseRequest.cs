namespace Bank.Issuer.Library.Libraries.Base
{
    public class BaseRequest
    {
        public string AccountId { get; set; }
        public string MessageType { get; set; }
        public string TransactionId { get; set; }
        public string Origin { get; set; }
        public string Amount { get; set; }
    }
}
