using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Models;
using Application.DBContext;

namespace Application.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Room> GetAllRooms()
        {
            return _context.Room.Include(d => d.CoworkingSpace).Include(d => d.CoworkingSpace.Manager).AsQueryable();
        }

        public Room GetRoomById(int id)
        {
            return _context.Room.Include(d => d.CoworkingSpace).Include(d => d.CoworkingSpace.Manager).FirstOrDefault(d => d.RoomId == id);
        }

        public IEnumerable<Room> GetRoomsByCoworkingSpaceId(int coworkingSpaceId)
        {
            return _context.Room
                    .Where(coworkingSpace => coworkingSpace.CoworkingSpace.CoworkingSpaceId == coworkingSpaceId)
                    .ToList();
        }

        public void CreateRoom(Room room)
        {
            _context.Room.Add(room);
            _context.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            var room = _context.Room.Find(id);
            if (room != null)
            {
                _context.Room.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}
