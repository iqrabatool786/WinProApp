using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WinProApp.Areas.Identity.Data
{


    public class WinProAppContext : IdentityDbContext<WinProAppUser>
    {
        public WinProAppContext(DbContextOptions<WinProAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.ApplyConfiguration(new WinPronUserEntityConfiguration());
        }
    }

    public class WinPronUserEntityConfiguration : IEntityTypeConfiguration<WinProAppUser>
    {
        public void Configure(EntityTypeBuilder<WinProAppUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(255).IsRequired(true);
            builder.Property(u => u.LastName).HasMaxLength(255).IsRequired(false);
            builder.Property(u => u.Status).HasDefaultValue(true);
            builder.Property(u => u.StoreId).IsRequired(true);
        }
    }
}