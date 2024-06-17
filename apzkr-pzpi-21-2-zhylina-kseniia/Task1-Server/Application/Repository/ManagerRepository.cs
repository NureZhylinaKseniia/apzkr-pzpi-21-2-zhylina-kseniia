using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Models;
using Application.DBContext;

namespace Application.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Manager> GetAllManagers()
        {
            return _context.Manager.AsQueryable();
        }

        public Manager GetManagerById(int id)
        {
            return _context.Manager.Find(id);
        }

        public Manager GetManagerByEmail(string email)
        {
            return _context.Manager.FirstOrDefault(a => a.Email == email);
        }

        public void CreateManager(Manager maganer)
        {
            _context.Manager.Add(maganer);
            _context.SaveChanges();
        }

        public void UpdateManager(Manager maganer)
        {
            _context.Entry(maganer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteManager(int id)
        {
            var maganer = _context.Manager.Find(id);
            if (maganer != null)
            {
                _context.Manager.Remove(maganer);
                _context.SaveChanges();
            }
        }
    }
}
