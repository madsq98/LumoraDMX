using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class LumoraContext : DbContext
    {
        public LumoraContext(DbContextOptions<LumoraContext> options) : base(options) 
        {
        }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
