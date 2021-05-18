using System;
using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Entities
{
    public class Transaction:BaseEntity
    {
        public decimal Amount { get; set; }

        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }

        public Guid MessageTypeId { get; set; }
        public virtual MessageType MessageType { get; set; }

        public Guid OriginId { get; set; }
        public virtual Origin Origin { get; set; }
    }
}
