using Microsoft.EntityFrameworkCore;
using Vex.Models;

namespace Vex.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<BranchModel> Branches { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubCategoryModel> SubCategories { get; set; }
        public DbSet<PriorityModel> Priorities { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BranchModel>()
                .HasOne(b => b.Company)
                .WithMany()
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubCategoryModel>()
                .HasOne(sc => sc.Category)
                .WithMany()
                .HasForeignKey(sc => sc.Category.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Company)
                .WithMany()
                .HasForeignKey(t => t.Company.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Branch)
                .WithMany()
                .HasForeignKey(t => t.Branch.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.Category.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.SubCategory)
                .WithMany()
                .HasForeignKey(t => t.SubCategory.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.Status.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.Priority)
                .WithMany()
                .HasForeignKey(t => t.Priority.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedTo.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedBy.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketModel>()
                .HasOne(t => t.UpdatedBy)
                .WithMany()
                .HasForeignKey(t => t.UpdatedBy.Id)
                .OnDelete(DeleteBehavior.Restrict);
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

        private void UpdateTimestamps()
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