using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AuthDBContext : DbContext
    {
        public DbSet<User> Users { get; set;}

        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
            
        }
    }
}
