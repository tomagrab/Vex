using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class BranchModel
    {
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required CompanyModel Company { get; set; }

        public BranchModel() {}

        public BranchModel(string name, CompanyModel company)
        {
            Id = Guid.NewGuid();
            Name = name;
            Company = company;
        }

        public static async Task<List<BranchModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Branches.ToListAsync();
        }

        public static async Task<BranchModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Branches.FindAsync(id);
        }

        public async Task CreateAsync(AppDbContext context)
        {
            context.Branches.Add(this);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Branches.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Branches.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}