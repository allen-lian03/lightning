using Lightning.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lightning.Domain.Configurations
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles", "account");
            builder.HasKey(u => new { u.UserId, u.RoleId }).HasName("pk_user_role_id");

            builder.Property(u => u.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            
            builder.Property(u => u.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
        }
    }
}