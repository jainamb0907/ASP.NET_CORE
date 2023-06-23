using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practical_17_Core.Models;
using Practical_17_Core.Repository;

namespace Practical_17_Core.Controllers
{
    [Route("students")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet("")]
        [Authorize(Policy = "BothRolePolicy")]
        public ActionResult Index()
        {
            var students = _studentRepository.GetStudents();
            var studentsViewModel = _mapper.Map<IEnumerable<Student>>(students);
            return View(studentsViewModel);
        }

        [HttpGet("details/{id}")]
        [Authorize(Policy = "BothRolePolicy")]
        public ActionResult Details(Guid id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student is not null)
            {
                var Student = _mapper.Map<Student>(student);
                return View(Student);
            }
            return View("_NotFound", new ErrorViewModel { ErrorMessage = "No student found with this Id" });
        }

        [HttpGet("create")]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(model);
                var insertedStudent = _studentRepository.CreateStudent(student);
                return RedirectToAction("Details", new { id = insertedStudent.Id });
            }
            return View();
        }

        [HttpGet("edit/{id}")]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Edit(Guid id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student is not null)
            {
                var Student = _mapper.Map<Student>(student);
                return View(Student);
            }
            return View("_NotFound", new ErrorViewModel { ErrorMessage = "No student found with this Id" });
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Edit(Student model)
        {
            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(model);
                var updatedStudent = _studentRepository.UpdateStudent(student);
                return RedirectToAction("Details", new { id = updatedStudent.Id });
            }
            return View();
        }

        [HttpGet("delete/{id}")]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Delete(Guid id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student is not null)
            {
                var Student = _mapper.Map<Student>(student);
                return View(Student);
            }
            return View("_NotFound", new ErrorViewModel { ErrorMessage = "No student found with this Id" });
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult Delete(Guid id, IFormCollection form)
        {
            _studentRepository.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }

}
