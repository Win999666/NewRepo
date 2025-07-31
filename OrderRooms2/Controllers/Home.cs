using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderRooms2.Context;
using OrderRooms2.Managers;
using OrderRooms2.ModelDto;
using OrderRooms2.Models;

namespace OrderRooms2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
       private readonly ManagerRoom _manager;
        public Home(ManagerRoom man)
        {      
            _manager = man;
        }
      

        [HttpGet("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
          return  _manager.GetAllUsers();
        }

        [HttpGet("GetAllRooms")]
        public IEnumerable<Room> GetAllRooms()
        {
           return _manager.GetAllRooms();
        }

        [HttpGet("GetRoomById")]
        public string GetRoomById(int id)
        {
            return _manager.GetRoomById(id);
        }

        [HttpPost("Registration")]
        public string Registration(UserDto dto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
          return  _manager.Registration(dto, ip);
        }

        [HttpPost("Authorization")]
        public string Authorization(UserDto dto)
        {
           return _manager.Authorization(dto);
        }

        [HttpPost("CreateRoom")]
        public string AddRoom(RoomDto room)
        {
            return _manager.AddRoom(room);
        }

        [HttpDelete("DeleteRoom")]
        public string DeleteRoom(int id)
        {
           return _manager.DeleteRoom(id);
        }

        [HttpPost("CreateBooking")]
        public string CreateBooking(int id, Booking booking)
        {

           return _manager.BookingRoom(id, booking);
        }
       
    }
}
