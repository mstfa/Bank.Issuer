﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bank.Issuer.Data.Extensions;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bank.Issuer.Data
{
    public class BankIssuerDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public BankIssuerDbContext(DbContextOptions<BankIssuerDbContext> options)
            : base(options)
        {


        }

        public BankIssuerDbContext(DbContextOptions<BankIssuerDbContext> options,
            ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Audit> Audit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(u => u.AccountNumber)
                .IsUnique();
            modelBuilder.Seed();
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }

        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            var temporaryAuditEntities = await AuditNonTemporaryProperties();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (temporaryAuditEntities != null)
            {
                await AuditTemporaryProperties(temporaryAuditEntities);
            }

            return result;
        }

        async Task<IEnumerable<Tuple<EntityEntry, Audit>>> AuditNonTemporaryProperties()
        {
            try
            {
                ChangeTracker.DetectChanges();
                var entitiesToTrack = ChangeTracker.Entries().Where(e => !(e.Entity is Audit) && e.State != EntityState.Detached && e.State != EntityState.Unchanged);
                AddTimestamps(entitiesToTrack);
                await Audit.AddRangeAsync(
                    entitiesToTrack.Where(e => !e.Properties.Any(p => p.IsTemporary)).Select(e => new Audit()
                    {
                        DateCreated = DateTime.Now.ToUniversalTime(),
                        TableName = e.Metadata.GetTableName(),
                        Action = Enum.GetName(typeof(EntityState), e.State),
                        DateTime = DateTime.Now.ToUniversalTime(),
                        KeyValues = JsonConvert.SerializeObject(e.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                        NewValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Added || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                        OldValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue).NullIfEmpty())
                    }).ToList()
                );

                //Return list of pairs of EntityEntry and ToolAudit  
                return entitiesToTrack.Where(e => e.Properties.Any(p => p.IsTemporary))
                     .Select(e => new Tuple<EntityEntry, Audit>(
                         e,
                     new Audit()
                     {
                         DateCreated = DateTime.Now.ToUniversalTime(),
                         TableName = e.Metadata.GetTableName(),
                         Action = Enum.GetName(typeof(EntityState), e.State),
                         DateTime = DateTime.Now.ToUniversalTime(),
                         NewValues = JsonConvert.SerializeObject(e.Properties.Where(p => !p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty())
                     }
                     )).ToList();
            }
            catch (Exception e)
            {
                //todo:write log
            }

            return null;
        }

        async Task AuditTemporaryProperties(IEnumerable<Tuple<EntityEntry, Audit>> temporaryEntities)
        {
            try
            {
                if (temporaryEntities != null && temporaryEntities.Any())
                {
                    await Audit.AddRangeAsync(
                        temporaryEntities.ForEach(t => t.Item2.KeyValues = JsonConvert.SerializeObject(t.Item1.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()))
                            .Select(t => t.Item2)
                    );
                    await SaveChangesAsync();
                }
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                //todo wrote log
            }

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        private void AddTimestamps(IEnumerable<EntityEntry> entities)
        {

            foreach (var entity in entities)
            {
                bool isImplement = entity.Entity.GetType().GetInterfaces().Contains(typeof(BaseEntity));
                if (isImplement)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((BaseEntity)entity.Entity).DateCreated = DateTime.Now;
                    }

                    ((BaseEntity)entity.Entity).DateLastUpdated = DateTime.Now;
                }
            }
        }

    }
}
