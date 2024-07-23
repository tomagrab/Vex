using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class TicketModel
    {
        public Guid Id { get; set; }
        public required string Contact { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required CompanyModel Company { get; set; }
        public required BranchModel Branch { get; set; }
        public required string Description { get; set; }
        public required CategoryModel Category { get; set; }
        public required SubCategoryModel SubCategory { get; set; }
        public required string Status { get; set; }
        public required string Priority { get; set; }
        public required string AssignedTo { get; set; }
        public required string CreatedBy { get; set; }
        public required string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
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