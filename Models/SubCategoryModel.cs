using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class SubCategoryModel
    {
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required CategoryModel Category { get; set; }

        public SubCategoryModel() {}

        public SubCategoryModel(string name, CategoryModel category)
        {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
        }

        public static async Task<List<SubCategoryModel>> GetAllAsync(AppDbContext context)
        {
            return await context.SubCategories.ToListAsync();
        }

        public static async Task<SubCategoryModel?> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.SubCategories.FindAsync(id);
        }

        public static async Task<List<SubCategoryModel>> GetByCategoryAsync(AppDbContext context, Guid categoryId)
        {
            return await context.SubCategories
                .Where(sc => sc.Category.Id == categoryId)
                .ToListAsync();
        }

        public async Task CreateAsync(AppDbContext context)
        {
            context.SubCategories.Add(this);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.SubCategories.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.SubCategories.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}