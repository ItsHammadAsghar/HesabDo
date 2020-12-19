using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HesabDo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HesabDo.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        //entities
        public DbSet<HesabAccount> HesabAccounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<HesabAccountUser> HesabAccountUsers { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.seed();

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<HesabAccountUser>()
                .HasKey(bc => new { bc.UserID, bc.HesabAccountID });
            modelBuilder.Entity<HesabAccountUser>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.HesabAccountUsers)
                .HasForeignKey(bc => bc.UserID);
            modelBuilder.Entity<HesabAccountUser>()
                .HasOne(bc => bc.HesabAccount)
                .WithMany(c => c.HesabAccountUsers)
                .HasForeignKey(bc => bc.HesabAccountID);


            modelBuilder.Entity<HesabAccount>()
            .HasMany(c => c.Deposits)
            .WithOne(e => e.HesabAccount);

            modelBuilder.Entity<HesabAccount>()
            .HasMany(c => c.Expenses)
            .WithOne(e => e.HesabAccount);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(c => c.Deposits)
            .WithOne(e => e.ApplicationUser);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(c => c.Expenses)
            .WithOne(e => e.ApplicationUser);

            ////Container Has many Components
            //modelBuilder.Entity<Container>()
            //.HasMany(c => c.Components)
            //.WithOne(e => e.Container);

            ////Component Has many Elements
            //modelBuilder.Entity<Component>()
            //.HasMany(c => c.Elements)
            //.WithOne(e => e.Component);

            ////Element Has many Traits
            //modelBuilder.Entity<Element>()
            //.HasMany(c => c.Traits)
            //.WithOne(e => e.Element);
        }

    }
}

