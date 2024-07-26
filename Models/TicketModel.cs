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
        public CompanyModel Company { get; set; } = new CompanyModel { Name = string.Empty };

        [Required]
        public BranchModel Branch { get; set; } = new BranchModel { Name = string.Empty, Company = new CompanyModel { Name = string.Empty } };

        [Required]
        public CategoryModel Category { get; set; } = new CategoryModel { Name = string.Empty };

        [Required]
        public SubCategoryModel SubCategory { get; set; } = new SubCategoryModel { Name = string.Empty, Category = new CategoryModel { Name = string.Empty } };

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string AssignedTo { get; set; } = string.Empty;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public string UpdatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public TicketModel() {}

        public TicketModel(
            string contact,
            string phone,
            string email,
            CompanyModel company,
            BranchModel branch,
            string description,
            CategoryModel category,
            SubCategoryModel subCategory,
            string assignedTo,
            string createdBy,
            string updatedBy)
        {
            Id = Guid.NewGuid();
            Contact = contact;
            Phone = phone;
            Email = email;
            Company = company;
            Branch = branch;
            Description = description;
            Category = category;
            SubCategory = subCategory;
            Status = "Open";
            Priority = "Low";
            AssignedTo = assignedTo;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
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