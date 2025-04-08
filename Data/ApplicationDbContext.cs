// ✅ ApplicationDbContext.cs (รวม FavoriteCars และ OwnedCars พร้อมกำหนดความสัมพันธ์)

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CombustionCarGuideWeb.Models;

namespace CombustionCarGuideWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // บอก EF ว่าให้ใช้ตาราง Brands เดิม ไม่ต้องสร้างใหม่
            modelBuilder.Entity<Brand>().ToTable("Brands");

            // Brand → Cars
            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Cars)
                .WithOne(c => c.Brand)
                .HasForeignKey(c => c.BrandId);


            // Brand → Cars
            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Cars)
                .WithOne(c => c.Brand)
                .HasForeignKey(c => c.BrandId);

            // Comment → User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment → Car
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Car)
                .WithMany(car => car.Comments)
                .HasForeignKey(c => c.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating → User
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating → Car
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal field precision
            modelBuilder.Entity<Car>().Property(c => c.MSRP).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Car>().Property(c => c.CostToDrive).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Car>().Property(c => c.CargoCapacity).HasColumnType("decimal(5,2)");



            // ✅ FavoriteCars: User ↔ Car (Many-to-Many)
            modelBuilder.Entity<Car>()
                .HasMany(c => c.FavoritedByUsers)
                .WithMany(u => u.FavoriteCars)
                .UsingEntity(j => j.ToTable("CarFavorites")); // ชื่อตารางกลาง

        }
    }
}