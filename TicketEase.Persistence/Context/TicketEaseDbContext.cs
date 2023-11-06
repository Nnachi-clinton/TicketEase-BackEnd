using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Common.Utilities;
using TicketEase.Domain.Entities;

namespace TicketEase.Persistence.Context
{
    public class TicketEaseDbContext : IdentityDbContext<User>
    {

        public TicketEaseDbContext(DbContextOptions<TicketEaseDbContext> options) 
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seeder.SeedData(modelBuilder);
        }
    }

   
}
 