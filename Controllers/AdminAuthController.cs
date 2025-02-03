using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sagetaway.Data;
using Sagetaway.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Sagetaway.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly SagetawayContext _context;

        public AdminAuthController(SagetawayContext context)
        {
            _context = context;
        }

        // Registration
        [AllowAnonymous] // Registration should be open to everyone
        public IActionResult AdminReg() => View();

        [HttpPost]
        public async Task<IActionResult> AdminReg(Admins admins)
        {
           
            ModelState.Remove(nameof(admins.PasswordHash));

            // Validate the model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                // Hash the password
                admins.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admins.PasswordInput);

                // Add the Admins entity to the DbContext
                _context.Admins.Add(admins);

                // Save changes
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok(new { success = true, message = "Registration successful!" });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Could not save data to the database." });
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                // Include inner exception details if available
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                }

                // Return 500 status code with detailed error message
                return StatusCode(500, new { success = false, message = $"An error occurred: {errorMessage}" });
            }
        }

        
        [AllowAnonymous]
        public IActionResult AdminLogin() => View();

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admins loginData)
        {
            ModelState.Remove(nameof(loginData.FullName));
            ModelState.Remove(nameof(loginData.PasswordHash));

            // Validate input
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                // Find the admin by email
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == loginData.Email);

                if (admin == null)
                {
                    return Unauthorized(new { success = false, message = "Invalid email or password." });
                }

                // Verify the password
                bool passwordMatches = BCrypt.Net.BCrypt.Verify(loginData.PasswordInput, admin.PasswordHash);

                if (!passwordMatches)
                {
                    return Unauthorized(new { success = false, message = "Invalid email or password." });
                }

                // Set up claims for the admin
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.FullName),
            new Claim(ClaimTypes.Email, admin.Email)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Set this based on your requirement
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set expiration time
                };

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                Response.Cookies.Append("AdminName", admin.FullName, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1)  
                });
                return Ok(new { success = true, message = "Login successful!", adminName = admin.FullName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }   // Dashboard (Protected)
        //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Dashboard()
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
            return View();
        }
        // Logout
        //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin", "AdminAuth");
        }
    }

}
