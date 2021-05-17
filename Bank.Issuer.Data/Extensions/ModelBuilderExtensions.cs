using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Issuer.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id=Guid.NewGuid(),
                    AccountNumber = 4755,
                    Balance = 1001.88
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    AccountNumber = 9834,
                    Balance = 456.45
                },
                new Account
                {
                    Id = Guid.NewGuid(),
                    AccountNumber = 7735,
                    Balance = 89.36
                }
            );

            modelBuilder.Entity<MessageType>().HasData(
                new MessageType
                {
                    Id = Guid.NewGuid(),
                    Type = "PAYMENT"
                },
                new MessageType
                {
                    Id = Guid.NewGuid(),
                    Type = "ADJUSTMENT"
                }
            );

            modelBuilder.Entity<Origin>().HasData(
                new Origin
                {
                    Id = Guid.NewGuid(),
                    Name = "VISA"
                },
                new MessageType
                {
                    Id = Guid.NewGuid(),
                    Type = "MASTER"
                }
            );
        }
    }
}
