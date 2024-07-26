using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class BranchModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public Guid CompanyId { get; set; }
        public CompanyModel Company { get; set; } = null!;

        [Required(ErrorMessage = "Branch name is required.")]
        public string Name { get; set; }

        public BranchModel() {}

        public BranchModel(string name, Guid companyId)
        {
            Id = Guid.NewGuid();
            Name = name;
            CompanyId = companyId;
        }

        public static async Task<List<BranchModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Branches.Include(b => b.Company).ToListAsync();
        }

        public static async Task<BranchModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Branches.Include(b => b.Company).FirstOrDefaultAsync(b => b.Id == id);
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