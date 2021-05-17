using System;

namespace Bank.Issuer.Library.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastUpdated { get; set; }
    }
}
