using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CinemaApp.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Conexiune directă, fără appsettings.json
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CinemaDB;Trusted_Connection=True;TrustServerCertificate=True;");


            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
