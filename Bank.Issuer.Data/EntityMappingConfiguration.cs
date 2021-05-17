using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Issuer.Data
{

    public abstract class EntityMappingConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> b);

        public void Map(ModelBuilder b)
        {
            Map(b.Entity<T>());
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            
        }
    }
}
