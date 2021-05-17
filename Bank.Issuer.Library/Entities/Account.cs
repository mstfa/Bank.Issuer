using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Entities
{
    public class Account:BaseEntity
    {
        public double Balance { get; set; }
        public long AccountNumber { get; set; }
    }
}
