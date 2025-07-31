using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderRooms2.Context;
using OrderRooms2.ModelDto;
using OrderRooms2.Models;

namespace OrderRooms2.Managers
{
    public class ManagerRoom
    {
       private readonly ApplicationDbContext _db;
        public ManagerRoom(ApplicationDbContext db)
        {
            _db = db;
        }

       
        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public string Authorization(UserDto user)
        {
            var checkUser = _db.Users.FirstOrDefault(item => item.NickName == user.NickName);
            if(checkUser is null||checkUser.Password != user.Password)
            {
                return "неверный nickName или password";
            }
            return $"Welcome {checkUser.NickName}";
        }
        public string Registration(UserDto dto, string ip)
        {
            var s = _db.Users.FirstOrDefault(item => item.NickName == dto.NickName);
            if(s is not null)
            {
                return "такой никнейм уже есть";
            }
            User user1 = new User();
            user1.Password = dto.Password;
            user1.IPAdress = ip;
            user1.NickName = dto.NickName;
            user1.Role = "client";
            _db.Users.Add(user1);
            _db.SaveChanges();
            return $"{user1.NickName} успешно  создан";
        }
        public string AddRoom(RoomDto room)
        {
            Room newRoom = new Room();
            newRoom.Description = room.Description;
            newRoom.Price = room.Price;
            _db.Rooms.Add(newRoom);
            _db.SaveChanges();
            return "успешно";
        }
        public string DeleteRoom(int id)
        {
           var room =  _db.Rooms.FirstOrDefault(item => item.Id == id);
            if(room is null)
            {
                return "не найдено комнаты с таким id";
            }
            _db.Rooms.Remove(room);
            _db.SaveChanges();
            return "успешно";
        }
        public IEnumerable<Room> GetAllRooms()
        {
            return _db.Rooms.Include(item => item.Bookings).ToList();
        }
        public string BookingRoom(int id, Booking booking)
        {
            var room = _db.Rooms.Include(item => item.Bookings).FirstOrDefault(item => item.Id == id);
            if (room is null)
            {
                return "нет такой комнаты";
            }
            room.Bookings.Add(booking);
            _db.SaveChanges();
            return $"{room.Description} add sucsesfull";
          
        }
        public string GetRoomById(int id)
        {
           var room = _db.Rooms.Include(item => item.Bookings).FirstOrDefault(item => item.Id == id);
            if( room is null)
            {
                return "нет такой комнаты";
            }
            string s = string.Empty;
            s += room.Description;
            s += "\n";
            foreach(var item in room.Bookings)
            {
                s += item.InDate;
                s += "\n";
                s += item.OutDate;
                s += "\n";
                s += "\n";
            }
            return s;
        }
    }
}
