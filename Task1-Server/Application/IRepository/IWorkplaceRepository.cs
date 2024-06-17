using Application.Models;

namespace Application.Repositories
{
    public interface IWorkplaceRepository
    {
        IQueryable<Workplace> GetAllWorkplaces();
        Workplace GetWorkplaceById(int id);
        IEnumerable<Workplace> GetWorkplacesByRoomId(int roomId);
        void CreateWorkplace(Workplace workplace);
        void UpdateWorkplace(Workplace workplace);
        void DeleteWorkplace(int id);
    }
}
