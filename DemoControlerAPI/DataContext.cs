using Microsoft.EntityFrameworkCore;
using DemoModels;

namespace DemoControlerAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source = DESKTOP-FQN6RGN; Initial Catalog=BookDB; Integrated Security = True; Connect Timeout = 30; Encrypt = True; Trust Server Certificate = True;");
        }
    }
}
