using System.ComponentModel.DataAnnotations;

namespace OrderRooms2.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime InDate { get; set; }
        public DateTime OutDate { get; set; }
        public int RoomId { get; set; }
       // public Room? Room { get; set; }
    }
}
