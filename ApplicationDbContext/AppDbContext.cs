using Knila_Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knila_Projects.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Contact> KnilaFullContactsTBL { get; set; }

        public DbSet<UserLogin> KnilaLoginTbl { get; set; }
        public DbSet<KnilaToken> KnilaTokenTbl { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogin>()
           .HasKey(u => u.UserId);   



            modelBuilder.Entity<KnilaToken>()
               .HasOne(k => k.User)
               .WithMany() 
               .HasForeignKey(k => k.UserId)
               .OnDelete(DeleteBehavior.Cascade); 
        }


    }
}
