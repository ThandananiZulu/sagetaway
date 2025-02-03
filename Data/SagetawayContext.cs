using Microsoft.EntityFrameworkCore;
using sagetaway.Models;
using Sagetaway.Models;

namespace Sagetaway.Data
{
    public class SagetawayContext : DbContext
    {
        public SagetawayContext(DbContextOptions<SagetawayContext> options) : base(options) { }

        public DbSet<Admins> Admins { get; set; }
        public DbSet<AdminHotel> AdminHotels { get; set; }

    }
}
