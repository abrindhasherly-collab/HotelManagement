using AutoMapper;
using HotelManagement.Dtos;
using HotelManagement.Entity;
using HotelManagement.Repository;

namespace HotelManagement.Service
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository repository;
        private readonly IMapper mapper;

        public HotelService(
            IHotelRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // CREATE
        public async Task<int> CreateHotelAsync(HotelDto hotelDto)
        {
            var entity = mapper.Map<Hotel>(hotelDto);

            return await repository.CreateHotelAsync(entity);
        }

        // GET ALL
        public async Task<IEnumerable<HotelDto>> GetHotelsAsync()
        {
            var entities = await repository.GetHotelsAsync();

            return mapper.Map<IEnumerable<HotelDto>>(entities);
        }

        // GET BY ID
        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            var entity = await repository.GetHotelByIdAsync(id);

            if (entity == null)
            {
                return null;
            }

            return mapper.Map<HotelDto>(entity);
        }

        // UPDATE
        public async Task<bool> UpdateHotelAsync(
            int id,
            HotelDto hotelDto)
        {
            var entity = mapper.Map<Hotel>(hotelDto);

            entity.Id = id;

            return await repository.UpdateHotelAsync(entity);
        }

        // DELETE
        public async Task<bool> DeleteHotelAsync(int id)
        {
            return await repository.DeleteHotelAsync(id);
        }

        // LOGIN
        public async Task<HotelDto?> LoginAsync(LoginDto loginDto)
        {
            var entity = await repository.LoginAsync(
                loginDto.Id,
                loginDto.HotelName);

            if (entity == null)
            {
                return null;
            }

            return mapper.Map<HotelDto>(entity);
        }
    }
}