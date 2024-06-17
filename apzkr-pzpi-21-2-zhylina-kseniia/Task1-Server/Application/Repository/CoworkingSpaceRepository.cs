using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Models;
using Application.DBContext;
using Application.Repositories;

namespace Application.Repositories
{
    public class CoworkingSpaceRepository : ICoworkingSpaceRepository
    {
        private readonly AppDbContext _context;

        public CoworkingSpaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<CoworkingSpace> GetAllCoworkingSpaces()
        {
            return _context.CoworkingSpace.Include(d => d.Manager).AsQueryable();
        }

        public CoworkingSpace GetCoworkingSpaceById(int id)
        {
            return _context.CoworkingSpace.Include(d => d.Manager).FirstOrDefault(d => d.CoworkingSpaceId == id);
        }

        public void AddCoworkingSpace(CoworkingSpace coworkingSpace)
        {
            _context.CoworkingSpace.Add(coworkingSpace);
            _context.SaveChanges();
        }

        public void AddCoworkingSpaceManager(CoworkingSpace coworkingSpace)
        {
            _context.Entry(coworkingSpace).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateCoworkingSpace(CoworkingSpace coworkingSpace)
        {
            _context.Entry(coworkingSpace).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCoworkingSpace(int id)
        {
            var coworkingSpace = _context.CoworkingSpace.FirstOrDefault(n => n.CoworkingSpaceId == id);

            if (coworkingSpace != null)
            {
                _context.CoworkingSpace.Remove(coworkingSpace);
                _context.SaveChanges();
            }
        }
    }
}
