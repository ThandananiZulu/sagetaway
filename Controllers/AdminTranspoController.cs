using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sagetaway.Models;
using Sagetaway.Data;    
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace sagetaway.Controllers
{

    public class AdminTranspoController : Controller
    {
        private readonly SagetawayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminTranspoController(SagetawayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var transpo = _context.AdminTranspo
                      .Where(h => h.Status == 1)
                      .ToList();

            return Json(new { data = transpo });
        }

        // POST: api/AdminHotels/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdminTranspo adminTranspo)
        {
            ModelState.Remove(nameof(adminTranspo.Image1));
            ModelState.Remove(nameof(adminTranspo.Image2));
            ModelState.Remove(nameof(adminTranspo.Image3));
            ModelState.Remove(nameof(adminTranspo.Image4));
            ModelState.Remove(nameof(adminTranspo.Image5));
            if (!ModelState.IsValid)
            {
                var friendlyFieldNames = new Dictionary<string, string>
                {
                    { "Img1", "Primary Image" },
                    { "Img2", "Secondary Image 1" },
                    { "Img3", "Secondary Image 2" },
                    { "Img4", "Secondary Image 3" },
                    { "Img5", "Secondary Image 4" },
                    { "Name", " Name" },
                    { "ContactNumber", "Contact Number" },
                    { "LocationName", "Location Name" },
                    { "Price", " Price" },
                    { "GoogleMapsEmbed", "Google Maps Embed Link" },
                    { "Information", "General Information" }
                };

                var errors = ModelState
                             .SelectMany(state => state.Value.Errors.Select(error =>
                                 new
                                 {
                                     Field = friendlyFieldNames.ContainsKey(state.Key) ? friendlyFieldNames[state.Key] : state.Key,
                                     Message = error.ErrorMessage
                                 }))
                             .Select(e => $"The {e.Field} is required.")
                             .ToList();

                return BadRequest(new { success = false, message = errors });
            }
            var imageFields = new (IFormFile img, string imageUrlField)[]
        {
            (adminTranspo.Img1, "Image1"),
            (adminTranspo.Img2, "Image2"),
            (adminTranspo.Img3, "Image3"),
            (adminTranspo.Img4, "Image4"),
            (adminTranspo.Img5, "Image5")
        };

            // Handle each image upload dynamically
            foreach (var (img, imageUrlField) in imageFields)
            {
                if (img != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "docs");

                    // Ensure the folder exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique file name to avoid conflicts
                    string fileName = Path.GetFileName(img.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file to the uploads folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    // Save the file URL in the corresponding Image field
                    var propertyInfo = adminTranspo.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        // Set the URL dynamically using reflection
                        propertyInfo.SetValue(adminTranspo, "/docs/" + fileName);
                    }
                }
            }
            try
            {
                // Add the AdminHotel entity to the DbContext
                _context.AdminTranspo.Add(adminTranspo);

                // Save changes to the database
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok(new { success = true, message = "Transport added successfully!" });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Could not save data to the database." });
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                }
                return StatusCode(500, new { success = false, message = $"An error occurred: {errorMessage}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] AdminTranspo adminTranspo)
        {
            // Ensure the hotel ID is set
            if (adminTranspo.Id == 0)
            {
                return BadRequest(new { success = false, message = "Invalid transport ID" });
            }
            ModelState.Remove(nameof(adminTranspo.Image1));
            ModelState.Remove(nameof(adminTranspo.Image2));
            ModelState.Remove(nameof(adminTranspo.Image3));
            ModelState.Remove(nameof(adminTranspo.Image4));
            ModelState.Remove(nameof(adminTranspo.Image5));
            ModelState.Remove(nameof(adminTranspo.Img1));
            ModelState.Remove(nameof(adminTranspo.Img2));
            ModelState.Remove(nameof(adminTranspo.Img3));
            ModelState.Remove(nameof(adminTranspo.Img4));
            ModelState.Remove(nameof(adminTranspo.Img5));
            if (!ModelState.IsValid)
            {
                var friendlyFieldNames = new Dictionary<string, string>
                {
                    { "Img1", "Primary Image" },
                    { "Img2", "Secondary Image 1" },
                    { "Img3", "Secondary Image 2" },
                    { "Img4", "Secondary Image 3" },
                    { "Img5", "Secondary Image 4" },
                    { "Name", " Name" },
                    { "ContactNumber", "Contact Number" },
                    { "LocationName", "Location Name" },
                    { "Price", " Price" },
                    { "GoogleMapsEmbed", "Google Maps Embed Link" },
                    { "Information", "General Information" }
                };

                var errors = ModelState
                             .SelectMany(state => state.Value.Errors.Select(error =>
                                 new
                                 {
                                     Field = friendlyFieldNames.ContainsKey(state.Key) ? friendlyFieldNames[state.Key] : state.Key,
                                     Message = error.ErrorMessage
                                 }))
                             .Select(e => $"The {e.Field} is required.")
                             .ToList();

                return BadRequest(new { success = false, message = errors });
            }
            // Find the existing hotel in the database
            var existingTranspo = await _context.AdminTranspo.FindAsync(adminTranspo.Id);
            if (existingTranspo == null)
            {
                return NotFound(new { success = false, message = "Transport not found" });
            }

            // Update the hotel properties
            existingTranspo.Name = adminTranspo.Name;
            existingTranspo.ContactNumber = adminTranspo.ContactNumber;
            existingTranspo.LocationName = adminTranspo.LocationName;
            existingTranspo.Price = adminTranspo.Price;
            existingTranspo.GoogleMapsEmbed = adminTranspo.GoogleMapsEmbed;
            existingTranspo.Information = adminTranspo.Information;

            // Handle image updates (if any)
            var imageFields = new (IFormFile img, string imageUrlField)[]
            {
        (adminTranspo.Img1, "Image1"),
        (adminTranspo.Img2, "Image2"),
        (adminTranspo.Img3, "Image3"),
        (adminTranspo.Img4, "Image4"),
        (adminTranspo.Img5, "Image5")
            };

            foreach (var (img, imageUrlField) in imageFields)
            {
                if (img != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "docs");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Path.GetFileName(img.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var propertyInfo = existingTranspo.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(existingTranspo, "/docs/" + fileName);
                    }
                }
            }

            // Save the changes
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Transpo updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
          
            var transpo = await _context.AdminTranspo
                .Where(h => h.Id == id && h.Status == 1) 
                .FirstOrDefaultAsync();

  
            if (transpo == null)
            {
                return NotFound(new { success = false, message = "Transpo not found or already deleted." });
            }

            transpo.Status = 0;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Transpo has been marked as deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error occurred: {ex.Message}" });
            }
        }

    }





}
