using Microsoft.AspNetCore.Mvc;
using University.MVC.Models;
using University.MVC.Services;

namespace University.MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentAPIService _departmentApiService;

        public DepartmentController(
            DepartmentAPIService departmentApiService)
        {
            _departmentApiService = departmentApiService;
        }

        public async Task<IActionResult> Index()
        {
            var departments =
                await _departmentApiService.GetAllDepartmentsAsync();

            return View(departments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DepartmentViewModel department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var success =
                await _departmentApiService
                    .CreateDepartmentAsync(department);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to create department."
                );

                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            DepartmentViewModel department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var success =
                await _departmentApiService
                    .UpdateDepartmentAsync(id, department);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to update department."
                );

                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success =
                await _departmentApiService
                    .DeleteDepartmentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
