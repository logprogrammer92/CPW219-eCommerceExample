﻿using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Controllers
{
    public class MemberController : Controller
    {
        private readonly ProductDbContext _context;

        public MemberController(ProductDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Member model tracked by DB
                Member newMember = new()
                {
                    Username = reg.Username,
                    Email = reg.Email,
                    Password = reg.Password,
                    DateOfBirth = reg.DateOfBirth
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                // Check if UsernameOrEmail and Password match a record in the database
                Member? loggedInMember = await _context.Members
                                     .Where(m => (m.Username == login.UsernameOrEmail || m.Email == login.UsernameOrEmail)
                                         && m.Password == login.Password)
                                         .SingleOrDefaultAsync();

                if (loggedInMember == null)
                {
                    ModelState.AddModelError(string.Empty, "Your provided credentials do not match any records in our database");
                    return View(login);
                }

                // Log the user in
                HttpContext.Session.SetString("Username", loggedInMember.Username);
                HttpContext.Session.SetInt32("Id", loggedInMember.MemberId);

                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }


        public IActionResult Logout()
            {
                // Destroy current session
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }
    }
