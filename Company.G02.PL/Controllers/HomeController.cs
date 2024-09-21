using System.Diagnostics;
using System.Text;
using Company.G02.PL.viewModels;
using Company.G02.PL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    // Controller responsible for handling Home-related actions
    public class HomeController : Controller
    {
        // Private readonly field for logging
        private readonly ILogger<HomeController> _logger;

        // Private readonly fields for scoped, transient, and singleton services
        private readonly IScopedService _scoped01;
        private readonly IScopedService _scoped02;
        private readonly ITransientService _transient01;
        private readonly ITransientService _transient02;
        private readonly ISingletonService _singleton01;
        private readonly ISingletonService _singleton02;

        // Constructor with dependency injection for ILogger and other services
        public HomeController(
            ILogger<HomeController> logger,
            IScopedService scoped01,
            IScopedService scoped02,
            ITransientService transient01,
            ITransientService transient02,
            ISingletonService singleton01,
            ISingletonService singleton02
        )
        {
            _logger = logger;
            _scoped01 = scoped01;
            _scoped02 = scoped02;
            _transient01 = transient01;
            _transient02 = transient02;
            _singleton01 = singleton01;
            _singleton02 = singleton02;
        }

        // GET /Home/TestLifeTime
        // Action method to test the lifetime of scoped, transient, and singleton services
        public string TestLifeTime()
        {
            // Use StringBuilder to format the GUIDs from different service lifetimes
            StringBuilder builder = new StringBuilder();

            builder.Append($"scoped01 :: {_scoped01.GetGuid()}\n");
            builder.Append($"scoped02 :: {_scoped02.GetGuid()}\n\n");

            builder.Append($"transient01 :: {_transient01.GetGuid()}\n");
            builder.Append($"transient02 :: {_transient02.GetGuid()}\n\n");

            builder.Append($"singleton01 :: {_singleton01.GetGuid()}\n");
            builder.Append($"singleton02 :: {_singleton02.GetGuid()}\n\n");

            // Return the formatted string as the response
            return builder.ToString();
        }

        // GET: /Home
        // Action method for the home page
        public IActionResult Index()
        {
            // Returns the default view for the Index action
            return View();
        }

        // GET: /Home/Privacy
        // Action method for the privacy page
        public IActionResult Privacy()
        {
            // Returns the default view for the Privacy action
            return View();
        }

        // GET: /Home/Error
        // Action method for handling errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Creates a new ErrorViewModel with the current request ID
            // Uses Activity.Current?.Id if available, otherwise falls back to HttpContext.TraceIdentifier
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
