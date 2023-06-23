using Practical_17_Core.Models;
using Practical_17_Core.Context;

namespace Practical_17_Core.Repository
{

    public class StudentRepository : IStudentRepository
    {
        private readonly StudentsDbContext _dbContext;
        public StudentRepository(StudentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Student CreateStudent(Student student)
        {
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            return student;
        }

        public void DeleteStudent(Guid id)
        {
            var student = GetStudentById(id);
            if (student is not null)
            {
                _dbContext.Students.Remove(student);
                _dbContext.SaveChanges();
            }
        }

        public Student? GetStudentById(Guid id)
        {
            return _dbContext.Students.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _dbContext.Students.ToList();
        }

        public Student UpdateStudent(Student student)
        {
            _dbContext.Students.Update(student);
            _dbContext.SaveChanges();
            return student;
        }
    }
}
