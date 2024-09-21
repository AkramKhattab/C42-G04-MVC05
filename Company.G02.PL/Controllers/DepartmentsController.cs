using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor Dependency Injection for UnitOfWork
        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            // The UnitOfWork is assigned here to interact with repositories via this single instance
            _unitOfWork = unitOfWork;
        }

        //--------- Index (List of Departments) ---------//
        // GET: Fetch and display all departments
        [HttpGet]
        public IActionResult Index()
        {
            // Get all departments from the repository
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);  // Pass departments to the View for rendering
        }

        //--------- Create (GET: Render Create Form) ---------//
        // GET: Render the form to create a new department
        [HttpGet]
        public IActionResult Create()
        {
            return View();  // Simply render the empty form
        }

        //--------- Create (POST: Handle Form Submission) ---------//
        // POST: Handle the form submission to create a new department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Add(model);  // Add the new department to the repository
                    var Count = _unitOfWork.Complete();  // Save changes to the database
                    TempData["Message"] = Count > 0 ? "Department Created" : "Department Not Created";
                    return RedirectToAction(nameof(Index));  // Redirect to the list of departments after creation
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);  // Show any error that occurs during execution
                }
            }
            return View(model);  // If validation fails, return the same form with the model
        }

        //--------- Details ---------//
        // GET: Display details of a department based on ID
        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            try
            {
                if (id == null) return BadRequest();  // Ensure ID is provided

                var department = _unitOfWork.DepartmentRepository.Get(id.Value);
                if (department == null) return NotFound();  // Return 404 if the department is not found

                return View(viewName, department);  // Render the details view
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);  // Handle any exception
                return RedirectToAction("Error", "Home");  // Redirect to error page
            }
        }

        //------------------ Edit (GET: Render Edit Form) ------------------//
        // GET: Render the form to edit an existing department
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return BadRequest();  // Check if ID is provided

                var department = _unitOfWork.DepartmentRepository.Get(id.Value);  // Fetch department using UnitOfWork
                if (department == null) return NotFound();  // 404 Not Found if the department does not exist

                return View(department);  // Return the department data for editing
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);  // Log and display the exception
                return RedirectToAction("Error", "Home");  // Redirect to the error page
            }
        }

        //--------- Edit (POST: Handle Form Submission) ---------//
        // POST: Update the department after form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, Department model)
        {
            try
            {
                if (id != model.Id) return BadRequest();  // 400 Bad Request if the ID doesn't match

                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.Update(model);  // Update the department
                    var Count = _unitOfWork.Complete();  // Commit the changes
                    if (Count > 0) return RedirectToAction(nameof(Index));  // If update is successful, redirect to Index
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);  // Log the exception
                return RedirectToAction("Error", "Home");  // Redirect to an error page
            }
            return View(model);  // If validation fails, return the same form
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id is null) return BadRequest();
                var department = _unitOfWork.DepartmentRepository.Get(id.Value);
                if (department is null) return NotFound();

                return View(department);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete([FromRoute] int? id, Department model)
        {
            try
            {
                if (id != model.Id) return BadRequest();
                if (ModelState.IsValid) 
                {
                    _unitOfWork.DepartmentRepository.Delete(model);
                    var Count = _unitOfWork.Complete();

                    if (Count >0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }







    }
}
