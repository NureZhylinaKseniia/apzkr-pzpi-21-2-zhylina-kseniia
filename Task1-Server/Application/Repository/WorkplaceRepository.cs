using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Models;
using Application.DBContext;

namespace Application.Repositories
{
    public class WorkplaceRepository : IWorkplaceRepository
    {
        private readonly AppDbContext _context;

        public WorkplaceRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Workplace> GetAllWorkplaces()
        {
            return _context.Workplaces.Include(d => d.Room).AsQueryable();
        }

        public Workplace GetWorkplaceById(int id)
        {
            return _context.Workplaces.Include(d => d.Room).FirstOrDefault(d => d.WorkplaceId == id);
        }

        public IEnumerable<Workplace> GetWorkplacesByRoomId(int roomId)
        {
            return _context.Workplaces
                    .Where(room => room.Room.RoomId == roomId)
                    .ToList();
        }

        public void CreateWorkplace(Workplace workplace)
        {
            _context.Workplaces.Add(workplace);
            _context.SaveChanges();
        }

        public void UpdateWorkplace(Workplace workplace)
        {
            _context.Entry(workplace).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteWorkplace(int id)
        {
            var workplace = _context.Workplaces.Find(id);
            if (workplace != null)
            {
                _context.Workplaces.Remove(workplace);
                _context.SaveChanges();
            }
        }
    }
}
