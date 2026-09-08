using HotelManagement.Data;
using HotelManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelManagementDbContext dbContext;

        public HotelRepository(HotelManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // CREATE
        public async Task<int> CreateHotelAsync(Hotel hotel)
        {
            await dbContext.Hotels.AddAsync(hotel);
            await dbContext.SaveChangesAsync();

            return hotel.Id;
        }

        // GET ALL
        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await dbContext.Hotels.ToListAsync();
        }

        // GET BY ID
        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await dbContext.Hotels
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        // UPDATE
        public async Task<bool> UpdateHotelAsync(Hotel hotel)
        {
            var existingHotel = await dbContext.Hotels
                .FirstOrDefaultAsync(h => h.Id == hotel.Id);

            if (existingHotel == null)
            {
                return false;
            }

            existingHotel.HotelName = hotel.HotelName;
            existingHotel.Location = hotel.Location;
            existingHotel.RoomType = hotel.RoomType;
            existingHotel.PricePerNight = hotel.PricePerNight;
            existingHotel.NumberOfRooms = hotel.NumberOfRooms;
            existingHotel.ContactNumber = hotel.ContactNumber;

            await dbContext.SaveChangesAsync();

            return true;
        }

        // DELETE
        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await dbContext.Hotels
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                return false;
            }

            dbContext.Hotels.Remove(hotel);

            await dbContext.SaveChangesAsync();

            return true;
        }

        // LOGIN
        public async Task<Hotel?> LoginAsync(
            int id,
            string hotelName)
        {
            return await dbContext.Hotels
                .FirstOrDefaultAsync(h =>
                    h.Id == id &&
                    h.HotelName == hotelName);
        }
    }
}