using Practical20.Models;
using Practical20.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Practical20.Controllers
{
    public class StudentController : Controller
    {
        public readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
        
        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
        }

        
        public async Task<IActionResult> GetAllStudents()
        {
            IEnumerable<Students> students =  await _studentService.GetAll();
            return View(students);
        }
        public async Task<ActionResult> AddStudent(int id)
        {
            if(id!= null)
            {
                var student = await _studentService.GetStudentbyId(id);
                return View(student);
            }
            return View();   
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(Students students)
        {
            if(students == null)
            {
                throw new Exception("Object reference not null Error");
            }
            if (ModelState.IsValid)
            {
                if(students.StudentId==0)
                {
                    await _studentService.AddStudent(students);
                }
                else if(students.StudentId!=0)
                {
                    await _studentService.UpdateStudent(students);
                }
                return RedirectToAction("GetAllStudents");
            }
           return View();
        }
        public async Task<IActionResult> DeleteStudent(int id) 
        {
            if(id==null)
            {
                throw new Exception("Object reference not null Error");
            }
            else
            {
                var student = await _studentService.GetStudentbyId(id);
                _studentService.DeleteStudent(student);
            }
            return RedirectToAction("GetAllStudents");
        }
    }
}
