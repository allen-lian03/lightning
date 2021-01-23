using Microsoft.EntityFrameworkCore;

using Lightning.Domain.Models;
using Lightning.Domain.Configurations;

namespace Lightning.Domain
{
    public class LightningContext : DbContext
    {
        public LightningContext(DbContextOptions<LightningContext> options) 
            : base(options)  { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}