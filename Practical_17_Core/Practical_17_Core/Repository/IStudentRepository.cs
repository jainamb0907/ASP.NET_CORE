using Practical_17_Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical_17_Core.Repository
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetStudents();
        Student? GetStudentById(Guid id);
        Student CreateStudent(Student student);
        Student UpdateStudent(Student student);
        void DeleteStudent(Guid id);
    }

}
