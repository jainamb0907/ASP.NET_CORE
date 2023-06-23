using Practical20.Models;

namespace Practical20.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Students>
    {
        Task<Students> GetStudentById(int studentId);
    }
}
