using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Core.Entities;

namespace TDS.Data
{
    public class TDSContext:DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<TransactionLedger> TransactionLedgers { get; set; }
        public TDSContext(DbContextOptions options) : base(options)
        {
            // this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasIndex(a => a.Account).IsUnique(true);
           
        }
    }
}
