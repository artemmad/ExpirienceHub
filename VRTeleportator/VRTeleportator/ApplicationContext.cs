using Microsoft.EntityFrameworkCore;
using VRTeleportator.Models;

namespace VRTeleportator
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<FileModel> Files { get; set; }
    }
}
