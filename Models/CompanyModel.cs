using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public CompanyModel() {}

        public CompanyModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static async Task<List<CompanyModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Companies.ToListAsync();
        }

        public static async Task<CompanyModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Companies.FindAsync(id);
        }

        public async Task CreateAsync(AppDbContext context)
        {
            context.Companies.Add(this);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Companies.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Companies.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}