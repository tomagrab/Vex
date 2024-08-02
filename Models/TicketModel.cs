using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class TicketModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Contact { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public CompanyModel Company { get; set; } = new CompanyModel { Name = string.Empty };

        [Required]
        public Guid BranchId { get; set; }
        public BranchModel Branch { get; set; } = new BranchModel { Name = string.Empty, Company = new CompanyModel { Name = string.Empty } };

        [Required]
        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; } = new CategoryModel { Name = string.Empty };

        [Required]
        public Guid SubCategoryId { get; set; }
        public SubCategoryModel SubCategory { get; set; } = new SubCategoryModel { Name = string.Empty, Category = new CategoryModel { Name = string.Empty } };

        [Required]
        public Guid StatusId { get; set; }
        public StatusModel Status { get; set; } = new StatusModel { Name = string.Empty };

        [Required]
        public Guid PriorityId { get; set; }
        public PriorityModel Priority { get; set; } = new PriorityModel { Name = string.Empty };

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string AssignedToId { get; set; }
        public UserModel AssignedTo { get; set; } = new UserModel { Name = string.Empty };

        [Required]
        public string CreatedById { get; set; }
        public UserModel CreatedBy { get; set; } = new UserModel { Name = string.Empty };

        [Required]
        public string UpdatedById { get; set; }
        public UserModel UpdatedBy { get; set; } = new UserModel { Name = string.Empty };

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public TicketModel() {}

        public TicketModel(
            string contact,
            string phone,
            string email,
            Guid companyId,
            Guid branchId,
            string description,
            Guid categoryId,
            Guid subCategoryId,
            Guid priorityId,
            Guid statusId,
            string assignedToId,
            string createdById,
            string updatedById)
        {
            Id = Guid.NewGuid();
            Contact = contact;
            Phone = phone;
            Email = email;
            CompanyId = companyId;
            BranchId = branchId;
            Description = description;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            StatusId = statusId;
            PriorityId = priorityId;
            AssignedToId = assignedToId;
            CreatedById = createdById;
            UpdatedById = updatedById;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static async Task<List<TicketModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Tickets.ToListAsync();
        }

        public static async Task<TicketModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Tickets.FindAsync(id);
        }

        public async Task CreateAsync(AppDbContext context)
        {
            context.Tickets.Add(this);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Tickets.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Tickets.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}