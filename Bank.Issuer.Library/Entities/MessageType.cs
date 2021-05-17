using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Entities.Base;

namespace Bank.Issuer.Library.Entities
{
    public class MessageType : BaseEntity
    {
        public string Type { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
