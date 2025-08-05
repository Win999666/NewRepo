using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderRooms2.Context;
using OrderRooms2.Managers;
using OrderRooms2.ModelDto;
using OrderRooms2.Models;
using System.Threading.Tasks;

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
        [Authorize]
        [HttpGet("GetAllRooms")]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
           return await _manager.GetAllRooms();
        }
        [Authorize]
        [HttpGet("GetRoomById")]
        public async Task<string> GetRoomById(int id)
        {
            return await _manager.GetRoomById(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateRoom")]
        public async Task<string> AddRoom(RoomDto room)
        {
            return await _manager.AddRoom(room);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteRoom")]
        public async Task<string> DeleteRoom(int id)
        {
           return await _manager.DeleteRoom(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteBooking")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            return await _manager.DeleteBooking(id);
        }

        [Authorize]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking(int id, BookingDto dto)
        {
           return await _manager.BookingRoom(id, dto);
        }
        
       
    }
}
