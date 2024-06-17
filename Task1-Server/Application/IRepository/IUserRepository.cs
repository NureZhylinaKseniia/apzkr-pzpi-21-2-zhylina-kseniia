using Application.Models;

namespace Application.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}