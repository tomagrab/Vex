using Vex.Models;
using Vex.Data;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<AppDbContext>();
        await AddCategoriesAndSubcategoriesAsync(context);
    }

    private static async Task AddCategoriesAndSubcategoriesAsync(AppDbContext context)
    {
        // Define categories and subcategories
        var categorySubcategories = new Dictionary<string, List<string>>
        {
            { "Mobile Application Support", new List<string> { "Application Error", "Application Freeze", "Device Setup", "Data Management", "General Question" } },
            { "OS Support", new List<string> { "Device Settings", "OS Error", "OS Freeze", "General Question" } },
            { "Connections", new List<string> { "WiFi Issues", "Cellular Data Connection Issue", "Activation/Deactivation", "DEX Issue" } },
            { "Handheld Device Issue", new List<string> { "Bluetooth Issue", "Hardware Failure, Misc.", "Power Issue", "Damaged Screen/Case" } },
            { "Printer Issue", new List<string> { "Pairing", "Training", "Damage", "Line in Print", "Power Issue" } },
            { "Depot Issue", new List<string> { "Asset Transfer", "New Device Order", "New Hardware Order", "New Accessory Order", "Return Accessories", "Asset/Accessory Failure", "Return (No Replacement)", "Loaner Return", "No Ticket Issued", "Terminated Employee", "Missing Items on Store Order", "New Upgrade Order" } },
            { "VCAM", new List<string> { "No Video Channel 1 (Forward Facing)", "No Video Channel 2 (Driver Facing)", "Video Distortion Channel 1 (Forward Facing)", "Video Distortion Channel 2 (Driver Facing)", "Power Issue", "WI-FI Issue", "Cellular Data Connection Issue", "No Video Caused by Wrong Settings" } },
            { "Middleware Support", new List<string> { "General Question", "Access Request", "Backend System Issue", "Document Issue/Export", "Data Management" } }
        };

        foreach (var categoryEntry in categorySubcategories)
        {
            var categoryName = categoryEntry.Key;
            var subcategories = categoryEntry.Value;

            // Check if the category already exists
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category == null)
            {
                // Add the category if it doesn't exist
                category = new CategoryModel { Name = categoryName };
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }

            // Add subcategories for the category
            foreach (var subcategoryName in subcategories)
            {
                if (!await context.SubCategories.AnyAsync(sc => sc.Name == subcategoryName && sc.Category.Id == category.Id))
                {
                    var subCategory = new SubCategoryModel
                    {
                        Name = subcategoryName,
                        Category = category
                    };
                    context.SubCategories.Add(subCategory);
                }
            }
        }

        // Save all changes
        await context.SaveChangesAsync();
    }
}