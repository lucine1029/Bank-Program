using Microsoft.EntityFrameworkCore;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Data
{
    internal class BankContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\.;Initial Catalog=Spelar_Du_In_BankDB;Integrated Security=True;Pooling=False");
        }
    }
}
