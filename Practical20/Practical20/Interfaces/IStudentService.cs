using Practical20.Models;

namespace Practical20.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Students>> GetAll();
        Task<Students> GetStudentbyId(int id);
        Task AddStudent(Students student);
        Task UpdateStudent(Students students);
        public void DeleteStudent(Students students);

    }
}
