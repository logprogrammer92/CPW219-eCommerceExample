using System.Diagnostics;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    /// <summary>
    /// Controller for managing the home page and privacy policy of the eCommerce application.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        /// <param name="logger">The logger to instantiate this object with.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; // Assign the logger to the private field
        }

        /// <summary>
        /// Displays the home page of the application.
        /// </summary>
        /// <returns>returns the view of the index page</returns>
        public IActionResult Index()
        {
            return View(); // Return the view for the index page
        }

        /// <summary>
        /// Displays the privacy policy page of the application.
        /// </summary>
        /// <returns>returns the view of the privacy page</returns>
        public IActionResult Privacy()
        {
            return View(); // Return the view for the privacy page
        }

        /// <summary>
        /// Handles errors that occur in the application.
        /// </summary>
        /// <returns>returns the shared error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // Assign the current request ID or trace identifier to the model
            });
        }
    }
}
