using Practical18.Model;
using Practical18.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Practical18.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IstudentRepository _studentRepository;

        public StudentController(IstudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public IActionResult ShowAllStudent()
        {
            var student = _studentRepository.GetAllStudents();
            return Ok(student);
        }
        [HttpGet("ShowSingleStudent/{id}")]
        public IActionResult ShowSingleStudent(int? id)
        {
            var student = _studentRepository.GetSingleStudent(id);
            if (student == null)
            {
                return StatusCode(404, "Sorry,record not found!!");
            }
            return Ok(student);
        }
        [HttpPost]
        public IActionResult AddStudentRecord(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                var result = _studentRepository.AddStudent(student);
                return Ok(result);
            }
            return NotFound("Sorry,Model state not valid!!");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudentRecord(int? id, StudentViewModel student)
        {
            var result = _studentRepository.UpdateStudent(id, student);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Sorry,record not found!!");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int? id)
        {
            var result = _studentRepository.DeleteStudent(id);
            if (result == null)
            {
                return NotFound("Sorry,record not found!!");
            }
            return Ok(result);
        }
    }
}
