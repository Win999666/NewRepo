namespace OrderRooms2.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();  
    }
}
