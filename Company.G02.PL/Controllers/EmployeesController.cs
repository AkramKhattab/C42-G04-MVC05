using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.viewModels.Employees;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    // Controller responsible for handling Employee-related operations
    public class EmployeesController : Controller
    {
        //-------------------------------------------------------------------------------------------------//
        // Dependency Injection Fields

        // Private readonly fields for dependency injection of repositories


        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Constructor that injects the dependencies (repositories) through DI
        public EmployeesController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            // Initialize the injected repositories
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //-------------------------------------------------------------------------------------------------//
        
        //-------------------------------------------------------------------------------------------------//
        // ViewData, ViewBag, TempData Usage Comments

        // View's Dictionary - Transfer Data From Action To View (One Way)
        // 1. ViewData: Transfers data from controller to view using dictionary syntax.
        // Example: ViewData["Data01"] = "Hello World From ViewData";

        // 2. ViewBag: Transfers data dynamically from controller to view.
        // Example: ViewBag.Data02 = "Hello World From ViewBag";

        // 3. TempData: Transfers data between requests (e.g., after redirect).
        // Example: TempData["Data03"] = "Hello World From TempData";
        //-------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------//
        // Employee List (Index Action)

        // GET: /Employees/Index
        public IActionResult Index(string InputSearch)
        {
            // Initialize an empty collection of employees
            var employees = Enumerable.Empty<Employee>();

            // If no search input is provided, retrieve all employees
            if (string.IsNullOrEmpty(InputSearch))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                // If search input is provided, filter employees by name
                employees = _unitOfWork.EmployeeRepository.GetByName(InputSearch);
            }


                var result =  _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            // Return the view with the retrieved employees
            return View(result);
        }
        //-------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------//
        // Create Employee (GET and POST Actions)

        // GET: /Employees/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Retrieve all departments for dropdown list
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            // Pass departments to the view using ViewData
            ViewData["departments"] = departments;

            // Return the Create view
            return View();
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    //Casting EmployeeViewModel (ViewModel) --> Employee (Model)
                    //Mapping :

                    // 1.Manual Mapping

                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Address = model.Address,
                    //    Name = model.Name,
                    //    Salary = model.Salary,
                    //    Age = model.Age,
                    //    HiringDate = model.HiringDate,
                    //    IsActive = model.IsActive,
                    //    WorkFor = model.WorkFor,
                    //    WorkForId = model.WorkForId,
                    //    Email = model.Email,
                    //    PhoneNumber = model.PhoneNumber,
                    //};


                    // 2. Auto Mapping
                    var employee = _mapper.Map<Employee>(model);

                     _unitOfWork.EmployeeRepository.Add(employee);
                    var Count = _unitOfWork.Complete();

                    if (Count > 0)
                    {
                        TempData["Message"] = "Employee Created!";
                    }
                    else
                    {
                        TempData["Message"] = "Employee Not Created!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1.Log Exception
                    // 2. Friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }    
            }

            // If model is invalid, return the Create view with the current model
            return View(model);
        }
        //-------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------//
        // View Employee Details

        // GET: /Employees/Details/{id}
        [HttpGet]
        public IActionResult Details(int? id)
        {
            try
            {
                // Check if id is provided
                if (id is null) return BadRequest();

                // Retrieve the employee by id
                var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

                // If employee is not found, return NotFound result
                if (employee is null) return NotFound();

                // Map Employee to EmployeeViewModel
                //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                //{
                //    Id = employee.Id,
                //    Address = employee.Address,
                //    Name = employee.Name,
                //    Salary = employee.Salary,
                //    Age = employee.Age,
                //    HiringDate = employee.HiringDate,
                //    IsActive = employee.IsActive,
                //    WorkFor = employee.WorkFor,
                //    WorkForId = employee.WorkForId,
                //    Email = employee.Email,
                //    PhoneNumber = employee.PhoneNumber,
                //};

               var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

                // Return the Details view with the employee data
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                // Handle exception and redirect to Error view
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        //-------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------//
        // Edit Employee (GET and POST Actions)

        // GET: /Employees/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                // Retrieve all departments for dropdown in Edit view
                var departments = _unitOfWork.DepartmentRepository.GetAll();
                ViewData["departments"] = departments;

                // Check if id is provided
                if (id is null) return BadRequest();

                // Retrieve employee by id
                var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
                if (employee is null) return NotFound();

                // Map Employee to EmployeeViewModel
                //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                //{
                //    Id = employee.Id,
                //    Address = employee.Address,
                //    Name = employee.Name,
                //    Salary = employee.Salary,
                //    Age = employee.Age,
                //    HiringDate = employee.HiringDate,
                //    IsActive = employee.IsActive,
                //    WorkFor = employee.WorkFor,
                //    WorkForId = employee.WorkForId,
                //    Email = employee.Email,
                //    PhoneNumber = employee.PhoneNumber,
                //};

                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

                // Return the Edit view with employee data
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                // Handle exception and redirect to Error view
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: /Employees/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            try
            {
                // Check if the route id matches the model id
                if (id != model.Id) return BadRequest();

                // If model is valid, attempt to update the employee
                if (ModelState.IsValid)
                {
                    // Map EmployeeViewModel to Employee
                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Address = model.Address,
                    //    Name = model.Name,
                    //    Salary = model.Salary,
                    //    Age = model.Age,
                    //    HiringDate = model.HiringDate,
                    //    IsActive = model.IsActive,
                    //    WorkFor = model.WorkFor,
                    //    WorkForId = model.WorkForId,
                    //    Email = model.Email,
                    //    PhoneNumber = model.PhoneNumber,
                    //};

                    var employee = _mapper.Map<Employee>(model);



                    // Update the employee in the repository
                    _unitOfWork.EmployeeRepository.Update(employee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        // Redirect to Index if update is successful
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception and add error to ModelState
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // Return to the Edit view with the current model if validation failed
            return View(model);
        }
        //-------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------//
        // Delete Employee (GET and POST Actions)

        // GET: /Employees/Delete/{id}
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                // Check if id is provided
                if (id is null) return BadRequest();

                // Retrieve employee by id
                var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
                if (employee is null) return NotFound();

                // Map Employee to EmployeeViewModel
                //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
                //{
                //    Id = employee.Id,
                //    Address = employee.Address,
                //    Name = employee.Name,
                //    Salary = employee.Salary,
                //    Age = employee.Age,
                //    HiringDate = employee.HiringDate,
                //    IsActive = employee.IsActive,
                //    WorkFor = employee.WorkFor,
                //    WorkForId = employee.WorkForId,
                //    Email = employee.Email,
                //    PhoneNumber = employee.PhoneNumber,
                //};


                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);


                // Return the Delete view with employee data
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                // Handle exception and redirect to Error view
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: /Employees/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            try
            {
                // Check if the route id matches the model id
                if (id != model.Id) return BadRequest();

                // If model is valid, attempt to delete the employee
                if (ModelState.IsValid)
                {
                    // Map EmployeeViewModel to Employee entity
                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Address = model.Address,
                    //    Name = model.Name,
                    //    Salary = model.Salary,
                    //    Age = model.Age,
                    //    HiringDate = model.HiringDate,
                    //    IsActive = model.IsActive,
                    //    WorkFor = model.WorkFor,
                    //    WorkForId = model.WorkForId,
                    //    Email = model.Email,
                    //    PhoneNumber = model.PhoneNumber,
                    //};

                    var employee = _mapper.Map<Employee>(model);


                    // Attempt to delete the employee
                    _unitOfWork.EmployeeRepository.Delete(employee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        // Redirect to Index if deletion is successful
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception and add error to ModelState
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // Return to the Delete view with the current model if validation failed
            return View(model);
        }
        //-------------------------------------------------------------------------------------------------//
    }
}
