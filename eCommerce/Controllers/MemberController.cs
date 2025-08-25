using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Controllers
{

    /// <summary>
    /// Controller for managing member registration and login functionality in the eCommerce application.
    /// </summary>
    public class MemberController : Controller
    {
        private readonly ProductDbContext _context;

        /// <summary>
        /// Initializes a new instance of the MemberController class with the specified database context.
        /// </summary>
        /// <param name="context">Instantiates the private context</param>
        public MemberController(ProductDbContext context)
        {
            _context = context; // Assign the provided context to the private field
        }

        /// <summary>
        /// Displays the registration view for a new member.
        /// </summary>
        /// <returns>Returns the registration view for a new member</returns>
        public IActionResult Register()
        {
            return View(); // Return the registration view for a new member
        }

        /// <summary>
        /// Registers a new member and saves it to the database.
        /// </summary>
        /// <param name="reg">The person to be registered to the database</param>
        /// <returns>Returns the member register view</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel reg)
        {

            if (ModelState.IsValid)
            {
                // Check if the username or email already exists in the database
                bool usernameTaken = await _context.Members
                    .AnyAsync(m => m.Username == reg.Username); // Check if the username already exists
                if (usernameTaken)
                {
                    ModelState.AddModelError(nameof(Member.Username), "Username already taken");
                }

                bool emailTaken = await _context.Members
                    .AnyAsync(m => m.Email == reg.Email); // Check if the email already exists
                
                if (emailTaken)
                {
                    ModelState.AddModelError(nameof(Member.Email), "Email already taken");
                }

                if (usernameTaken || emailTaken)
                {
                    return View(reg); // Return the registration view with an error message if username or email is taken
                }

                // Map ViewModel to Member model tracked by DB
                Member newMember = new()
                {
                    Username = reg.Username, // Alphanumeric characters only
                    Email = reg.Email,
                    Password = reg.Password,
                    DateOfBirth = reg.DateOfBirth
                };
                

            }

            return View(reg); // If model state is invalid, return the view with the registration data and validation errors
        }

        /// <summary>
        /// Displays the login view for an existing member.
        /// </summary>
        /// <returns>returns the login view for an existing member</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Return the login view for an existing member
        }

        /// <summary>
        /// Logs in an existing member using their username or email and password.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Returns the member login view</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                // Check if UsernameOrEmail and Password match a record in the database
                Member? loggedInMember = await _context.Members
                                     .Where(m => (m.Username == login.UsernameOrEmail 
                                     || m.Email == login.UsernameOrEmail) // Check if UsernameOrEmail matches either Username or Email
                                         && m.Password == login.Password) 
                                         .SingleOrDefaultAsync(); // SingleOrDefaultAsync() returns a single record or null if no match is found

                if (loggedInMember == null) 
                {
                    ModelState.AddModelError(string.Empty, "Your provided credentials do " +
                        "not match any records in our database"); // Add an error to the model state if no match is found
                    return View(login); // Return the login view with an error message if no match is found
                }

                // Log the user in
                HttpContext.Session.SetString("Username", loggedInMember.Username);
                HttpContext.Session.SetInt32("Id", loggedInMember.MemberId);

                return RedirectToAction("Index", "Home"); // Redirect to the home page after successful login
            }
            return View(login); // If model state is invalid, return the view with the login data and validation errors
        }

        /// <summary>
        /// Logs out the current member by clearing the session and redirecting to the home page.
        /// </summary>
        /// <returns>Returns the view to the home page</returns>
        public IActionResult Logout()
            {
                // Destroy current session
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home"); // Redirect to the home page after logout
        }
        }
    }
