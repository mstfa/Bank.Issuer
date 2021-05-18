using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bank.Issuer.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Issuer.Data
{
    public class SeedDb
    {
        public static void Initialize(BankIssuerDbContext context)
        {
            // Look for any board games.
            if (!context.Account.Any())
            {
                context.Account.AddRange(
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        AccountNumber = 4755,
                        Balance = 1001.88,
                        DateCreated = DateTime.Now
                    },
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        AccountNumber = 9834,
                        Balance = 456.45,
                        DateCreated = DateTime.Now
                    },
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        AccountNumber = 7735,
                        Balance = 89.36,
                        DateCreated = DateTime.Now
                    });

                context.SaveChanges(); 
            }

            if (!context.MessageType.Any())
            {
                context.MessageType.AddRange(new MessageType
                    {
                        Id = Guid.NewGuid(),
                        Type = "PAYMENT",
                        DateCreated = DateTime.Now
                },
                    new MessageType
                    {
                        Id = Guid.NewGuid(),
                        Type = "ADJUSTMENT",
                        DateCreated = DateTime.Now
                    });
                context.SaveChanges();

            }
            Guid originVisa = Guid.NewGuid();
            Guid originMaster = Guid.NewGuid();

            if (!context.Origin.Any())
            {
                context.Origin.AddRange(new Origin
                    {
                        Id = originVisa,
                        Name = "VISA",
                        Rate = 1,
                        DateCreated = DateTime.Now
                },
                    new Origin
                    {
                        Id = originMaster,
                        Name = "MASTER",
                        Rate = 2,
                        DateCreated = DateTime.Now
                    });
                context.SaveChanges();
            }
        }
    }
}
