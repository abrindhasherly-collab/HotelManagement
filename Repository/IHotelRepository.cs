using HotelManagement.Entity;

namespace HotelManagement.Repository
{
    public interface IHotelRepository
    {
        Task<int> CreateHotelAsync(Hotel hotel);

        Task<IEnumerable<Hotel>> GetHotelsAsync();

        Task<Hotel?> GetHotelByIdAsync(int id);

        Task<bool> UpdateHotelAsync(Hotel hotel);

        Task<bool> DeleteHotelAsync(int id);

        Task<Hotel?> LoginAsync(int id, string hotelName);
    }
}