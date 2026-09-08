
using HotelManagement.Dtos;
using HotelManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Data
{
    public class HotelManagementDbContext : DbContext
    {
        public HotelManagementDbContext(
            DbContextOptions<HotelManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
    }
}