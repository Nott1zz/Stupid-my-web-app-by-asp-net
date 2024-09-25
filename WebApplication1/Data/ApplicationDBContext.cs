using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace WebApplication1.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option):base(option)
        {

        }
        public DbSet<Models.User>User { get; set; }
        

    }
}
