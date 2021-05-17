using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Issuer.Data.Mappings
{
    public class TransactionMap : EntityMappingConfiguration<Transaction>
    {
        public override void Map(EntityTypeBuilder<Transaction> b)
        {
            b.HasKey(p => p.Id);
            b.HasOne<MessageType>(o => o.MessageType)
                .WithMany(g => g.Transactions)
                .HasForeignKey(s => s.MessageTypeId);
        }
    }
}
