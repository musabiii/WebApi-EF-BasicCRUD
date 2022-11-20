using BasicCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicCRUD.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }

        public DbSet<Student>? Students { get; set; }
    }
}
