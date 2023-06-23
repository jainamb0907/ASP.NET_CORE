using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Practical18.Models;

namespace Practical18MVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        public StudentsController(IConfiguration configuration)
        {
            client.BaseAddress = new Uri(configuration["AppSettings:BaseUrl"]);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("/api/Student");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<StudentModel> students = JsonConvert.DeserializeObject<IEnumerable<StudentModel>>(content)!;
                return View(students);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Student/ShowSingleStudent/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                StudentModel students = JsonConvert.DeserializeObject<StudentModel>(content)!;
                return View("Details", students);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(StudentModel model)
        {
            if (DateTime.Now.Year - model.Dob.Year < 6)
            {
                ModelState.AddModelError("Invaliddate", "Student age must be greater than 6.");
            }
            if (ModelState.IsValid)
            {
                if (model.StudentId == null)
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync<StudentModel>("/api/student", model);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync<StudentModel>($"/api/student/{model.StudentId}", model);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View("Create");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Student/ShowSingleStudent/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                StudentModel students = JsonConvert.DeserializeObject<StudentModel>(content)!;
                return View("Create", students);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/Student/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
