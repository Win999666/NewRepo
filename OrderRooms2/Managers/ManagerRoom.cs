using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderRooms2.Context;
using OrderRooms2.ModelDto;
using OrderRooms2.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OrderRooms2.Managers
{
    public class ManagerRoom
    {
        private readonly ApplicationDbContext _db;
        public ManagerRoom(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string> AddRoom(RoomDto room)
        {
            Room newRoom = new Room();
            newRoom.Description = room.Description;
            newRoom.Price = room.Price;
            await _db.Rooms.AddAsync(newRoom);
            await _db.SaveChangesAsync();
            return "успешно";
        }
        public async Task<string> DeleteRoom(int id)
        {
            var room = await _db.Rooms.FirstOrDefaultAsync(item => item.Id == id);
            if (room is null)
            {
                return "не найдено комнаты с таким id";
            }
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return "успешно";
        }
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _db.Rooms.Include(item => item.Bookings).ToListAsync();
        }
        public async Task<ActionResult> BookingRoom(int id, BookingDto booking)
        {
            var room = await _db.Rooms.Include(item => item.Bookings).FirstOrDefaultAsync(item => item.Id == id);
            if (room is null)
            {
                return new BadRequestObjectResult("нет такой комнаты");
            }
            //var book = new Booking()
            //{
            //    InDate = booking.DateIn.ToDateTime(TimeOnly.MaxValue),
            //    OutDate = booking.DateOut.ToDateTime(TimeOnly.MinValue),
            //    RoomId = id
            //};
            var book = new Booking()
            {
                // Преобразуем DateOnly в DateTime, затем явно устанавливаем Kind в Utc
                // DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                InDate = DateTime.SpecifyKind(booking.DateIn.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
                OutDate = DateTime.SpecifyKind(booking.DateOut.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
                RoomId = id
            };
            var check = await CheckBookings(id, book);
            if (check == false)
            {
                return new BadRequestObjectResult("date invalid");
            }

            room.Bookings.Add(book);
            await _db.SaveChangesAsync();
            return new OkObjectResult("Sucsesful");

        }
        public async Task<string> GetRoomById(int id)
        {
            var room = await _db.Rooms.Include(item => item.Bookings).FirstOrDefaultAsync(item => item.Id == id);
            if (room is null)
            {
                return "нет такой комнаты";
            }
            string s = string.Empty;
            s += room.Description;
            s += "\n";
            foreach (var item in room.Bookings)
            {
                s += item.InDate;
                s += "\n";
                s += item.OutDate;
                s += "\n";
                s += "\n";
            }
            return s;
        }

        public async Task<ActionResult> DeleteBooking(int id)
        {
            var book = await _db.Bookings.FirstOrDefaultAsync(item => item.Id == id);
            if (book is null)
            {
                return new BadRequestObjectResult("id error");
            }
            _db.Bookings.Remove(book);
            await _db.SaveChangesAsync(true);
            return new OkObjectResult("ok");
        }
        public async Task<bool> CheckBookings(int idRoom, Booking book)
        {
            var room = await _db.Rooms.FirstOrDefaultAsync(item => item.Id == idRoom);
            if (room is null)
            {
                return false;
            }
            List<Booking> bookings =  (from item in await _db.Bookings.ToListAsync()
                                      where item.RoomId == room.Id
                                      select item).ToList();

            foreach (var booking in bookings)
            {
                if (book.InDate <= booking.OutDate && book.OutDate >= booking.InDate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
