using Microsoft.EntityFrameworkCore;
using PlaceApp.Models;

namespace PlaceApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=192.168.100.40;database=Muhammet;Persist Security Info=True;TrustServerCertificate=True;User ID=Logo;Password=JnRpndvJ;");
        }

        public DbSet<Places> Places { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<PlaceLists> PlaceLists { get; set; }
        public DbSet<SharePlace> SharePlace { get; set; }
        public DbSet<SharePlaceList> SharePlaceList { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Comment> Comment { get; set; }

        public DbSet<LikeUserViewModel> LikeUserViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<LikeUserViewModel>()
                .HasNoKey()
                .ToView("YourStoredProcedureName");

            base.OnModelCreating(modelBuilder);
        }
    }


}
