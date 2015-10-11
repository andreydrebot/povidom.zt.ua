using System.Data.Entity;
using povidom.Models;

namespace povidom
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<Problem> Problems { get; set; }

    }
}