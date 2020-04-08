using System;
using System.Collections.Generic;
using System.Text;
using BudgetiFi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetiFi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<DebtCategory> DebtCategories { get; set; }
        public DbSet<CreditorCategory> CreditorCategories { get; set; }
        public DbSet<DebtDetail> DebtDetails { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
    }
}
