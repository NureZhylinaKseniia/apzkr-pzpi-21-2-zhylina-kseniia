using Application.Models;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Application.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<LoginView> LoginView { get; set; }
        public DbSet<CoworkingSpace> CoworkingSpace { get; set; }
    }
}