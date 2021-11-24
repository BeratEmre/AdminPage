using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class AdminSql:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AdminPage;Trusted_Connection=true");
        }
       public   DbSet<UserForRegisterDto> Users { get; set; }
       public   DbSet<OperationClaim> OperationClaims { get; set; }
       public   DbSet<UserOperationClaim> UserOperationClaims { get; set; }

    }
}
