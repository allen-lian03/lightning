using Lightning.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lightning.Domain.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "account");
            builder.HasKey(u => u.ID).HasName("pk_user_id");

            builder.Property(u => u.ID)
                .HasColumnName("user_id")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            
            builder.Property(u => u.Name)
                .HasColumnName("user_name")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(u => u.Roles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .HasConstraintName("fk_user_role");

        }
    }
}