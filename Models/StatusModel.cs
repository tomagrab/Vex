using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Vex.Data;

namespace Vex.Models
{
    public class StatusModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public StatusModel() {}

        public StatusModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static async Task<List<StatusModel>> GetAllAsync(AppDbContext context)
        {
            return await context.Statuses.ToListAsync();
        }

        public static async Task<StatusModel> GetByIdAsync(AppDbContext context, Guid id)
        {
            return await context.Statuses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<StatusModel> GetByNameAsync(AppDbContext context, string name)
        {
            return await context.Statuses.FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task<StatusModel> CreateAsync(AppDbContext context, string name)
        {
            var status = new StatusModel{ Name = name };
            await context.Statuses.AddAsync(status);
            await context.SaveChangesAsync();
            return status;
        }

        public async Task UpdateAsync(AppDbContext context)
        {
            context.Statuses.Update(this);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AppDbContext context)
        {
            context.Statuses.Remove(this);
            await context.SaveChangesAsync();
        }
    }
}