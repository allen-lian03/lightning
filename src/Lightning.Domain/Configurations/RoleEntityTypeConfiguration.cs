using Lightning.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lightning.Domain.Configurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles", "account");
            builder.HasKey(r => r.ID).HasName("pk_role_id");

            builder.Property(r => r.ID)
                .HasColumnName("role_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(r => r.Name)
                .HasColumnName("role_name")
                .HasMaxLength(128)
                .IsRequired();

            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(r => r.RoleId)
                .HasConstraintName("fk_role_user");
        }
    }
}