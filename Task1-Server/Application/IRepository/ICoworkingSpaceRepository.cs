using Application.Models;

namespace Application.Repositories
{
    public interface ICoworkingSpaceRepository
    {
        IQueryable<CoworkingSpace> GetAllCoworkingSpaces();
        CoworkingSpace GetCoworkingSpaceById(int id);
        void AddCoworkingSpace(CoworkingSpace coworkingSpace);
        void AddCoworkingSpaceManager(CoworkingSpace coworkingSpace);
        void UpdateCoworkingSpace(CoworkingSpace coworkingSpace);
        void DeleteCoworkingSpace(int id);
    }
}
