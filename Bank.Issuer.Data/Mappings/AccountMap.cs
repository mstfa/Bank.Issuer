using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Issuer.Data.Mappings
{
    public class AccountMap : EntityMappingConfiguration<Account>
    {
        public override void Map(EntityTypeBuilder<Account> b)
        {
            b.HasKey(p => p.Id);
        }
    }
}
