using Application.Models;

namespace Application.Repositories
{
    public interface IRoomRepository
    {
        IQueryable<Room> GetAllRooms();
        Room GetRoomById(int id);
        IEnumerable<Room> GetRoomsByCoworkingSpaceId(int coworkingSpaceId);
        void CreateRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
    }
}
