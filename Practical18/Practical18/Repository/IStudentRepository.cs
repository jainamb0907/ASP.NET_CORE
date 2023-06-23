using Practical18.Model;

namespace Practical18.Repository
{
    public interface IstudentRepository
    {
        IEnumerable<Student> GetAllStudents();
        Student GetSingleStudent(int? id);
        IEnumerable<Student> AddStudent(StudentViewModel request);
        Student UpdateStudent(int? id, StudentViewModel student);
        Student DeleteStudent(int? id);
    }
}
