using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Menus.Data
{
    public class MenuContextFactory : IDesignTimeDbContextFactory<MenuContext>
    {
        public MenuContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MenuContext>();

            // Use the same connection string as in appsettings.json
            optionsBuilder.UseNpgsql("Host=localhost;Database=MenuDb;Username=postgres;Password=lora");

            return new MenuContext(optionsBuilder.Options);
        }
    }
}
