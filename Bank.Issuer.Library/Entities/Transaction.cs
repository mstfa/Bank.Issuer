using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Entities
{
    public class Transaction:BaseEntity
    {
        public decimal Amount { get; set; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public int MessageTypeId { get; set; }
        public virtual MessageType MessageType { get; set; }

        public int OriginId { get; set; }
        public virtual Origin Origin { get; set; }
    }
}
