using Microsoft.EntityFrameworkCore;
using Vex.Models;

namespace Vex.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BranchModel> Branches { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubCategoryModel> SubCategories { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is TicketModel &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((TicketModel)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                ((TicketModel)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}