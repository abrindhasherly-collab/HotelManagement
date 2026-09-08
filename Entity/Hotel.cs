namespace HotelManagement.Entity
{
    public class Hotel
    {
        public int Id { get; set; }

        public required string HotelName { get; set; }

        public required string Location { get; set; }

        public required string RoomType { get; set; }

        public decimal PricePerNight { get; set; }

        public int NumberOfRooms { get; set; }

        public required string ContactNumber { get; set; }
    }
}