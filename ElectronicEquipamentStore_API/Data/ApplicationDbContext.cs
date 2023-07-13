using ElectronicEquipamentStore_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicEquipamentStore_API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ElectronicEquipament> Equipaments { get; set; }
    }
}
