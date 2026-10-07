using Microsoft.AspNetCore.Mvc;
using University.MVC.Models;
using University.MVC.Services;

namespace University.MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentApiService _studentApiService;

        public StudentController(StudentApiService studentApiService)
        {
            _studentApiService = studentApiService;
        }

        // GET: /Student
        public async Task<IActionResult> Index()
        {
            var students = await _studentApiService.GetAllStudentsAsync();

            return View(students);
        }

        // GET: /Student/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentApiService.GetStudentAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: /Student/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            var success = await _studentApiService.CreateStudentAsync(student);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to create student."
                );

                return View(student);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentApiService.GetStudentAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: /Student/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            var success =
                await _studentApiService.UpdateStudentAsync(id, student);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to update student."
                );

                return View(student);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/Delete/1
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentApiService.GetStudentAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: /Student/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success =
                await _studentApiService.DeleteStudentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
