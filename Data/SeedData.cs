
using Microsoft.EntityFrameworkCore;
using Wger.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Wger.Api.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Languages
            if (!context.Languages.Any())
            {
                context.Languages.AddRange(
                    new Language { ShortName = "en", FullName = "English", FullNameEn = "English" },
                    new Language { ShortName = "de", FullName = "Deutsch", FullNameEn = "German" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Gyms
            if (!context.Gyms.Any())
            {
                context.Gyms.AddRange(
                    new Gym { Name = "Default Gym" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}

