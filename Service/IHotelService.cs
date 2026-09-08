using HotelManagement.Dtos;

namespace HotelManagement.Service
{
    public interface IHotelService
    {
        Task<int> CreateHotelAsync(HotelDto hotelDto);

        Task<IEnumerable<HotelDto>> GetHotelsAsync();

        Task<HotelDto?> GetHotelByIdAsync(int id);

        Task<bool> UpdateHotelAsync(int id, HotelDto hotelDto);

        Task<bool> DeleteHotelAsync(int id);

        Task<HotelDto?> LoginAsync(LoginDto loginDto);
    }
}