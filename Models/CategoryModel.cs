using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public CategoryModel() {}

        public CategoryModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static async Task<List<CategoryModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Categories.ToListAsync();
        }

        public static async Task<CategoryModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Categories.FindAsync(id);
        }

        public async Task CreateAsync(AppDbContext context)
        {
            context.Categories.Add(this);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Categories.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Categories.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}