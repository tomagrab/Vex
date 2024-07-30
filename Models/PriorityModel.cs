using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class PriorityModel
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }

        public PriorityModel() {}

        public PriorityModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static async Task<List<PriorityModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Priorities.ToListAsync();
        }

        public static async Task<PriorityModel> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Priorities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<PriorityModel> GetByNameAsync(AppDbContext context, string name)
        {
            return await context.Priorities.FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task<PriorityModel> CreateAsync(AppDbContext context, string name)
        {
            var priority = new PriorityModel{ Name = name };
            await context.Priorities.AddAsync(priority);
            await context.SaveChangesAsync();
            return priority;
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Priorities.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Priorities.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}