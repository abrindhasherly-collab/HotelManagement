namespace HotelManagement.Dtos
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }

        public decimal PricePerNight { get; set; }

        public int NumberOfRooms { get; set; }

        public string ContactNumber { get; set; }
    }
}