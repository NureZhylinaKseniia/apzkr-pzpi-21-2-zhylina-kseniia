using Application.Models;

namespace Application.Repositories
{
    public interface IManagerRepository
    {
        IQueryable<Manager> GetAllManagers();
        Manager GetManagerById(int id);
        Manager GetManagerByEmail(string email);
        void CreateManager(Manager manager);
        void UpdateManager(Manager manager);
        void DeleteManager(int id);
    }
}
