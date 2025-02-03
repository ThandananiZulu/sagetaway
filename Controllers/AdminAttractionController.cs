
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

    public class AdminAttractionController : Controller
    {
        private readonly SagetawayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminAttractionController(SagetawayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var attractions = _context.AdminAttraction
                      .Where(h => h.Status == 1)
                      .ToList();

            return Json(new { data = attractions });
        }

        // POST: api/AdminAttraction/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdminAttraction adminAttraction)
        {
            ModelState.Remove(nameof(adminAttraction.Image1));
            ModelState.Remove(nameof(adminAttraction.Image2));
            ModelState.Remove(nameof(adminAttraction.Image3));
            ModelState.Remove(nameof(adminAttraction.Image4));
            ModelState.Remove(nameof(adminAttraction.Image5));
            if (!ModelState.IsValid)
            {
                var friendlyFieldNames = new Dictionary<string, string>
                {
                    { "Img1", "Primary Image" },
                    { "Img2", "Secondary Image 1" },
                    { "Img3", "Secondary Image 2" },
                    { "Img4", "Secondary Image 3" },
                    { "Img5", "Secondary Image 4" },
                    { "Name", "Place Name" },
                    { "ContactNumber", "Contact Number" },
                    { "LocationName", "Location Name" },
                    { "Price", "Attraction Price" },
                    { "GoogleMapsEmbed", "Google Maps Embed Link" },
                    { "Information", "Attraction Information" }
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
            (adminAttraction.Img1, "Image1"),
            (adminAttraction.Img2, "Image2"),
            (adminAttraction.Img3, "Image3"),
            (adminAttraction.Img4, "Image4"),
            (adminAttraction.Img5, "Image5")
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
                    var propertyInfo = adminAttraction.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        // Set the URL dynamically using reflection
                        propertyInfo.SetValue(adminAttraction, "/docs/" + fileName);
                    }
                }
            }
            try
            {
                // Add the AdminAttraction entity to the DbContext
                _context.AdminAttraction.Add(adminAttraction);

                // Save changes to the database
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok(new { success = true, message = "Attraction added successfully!" });
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
        public async Task<IActionResult> Update([FromForm] AdminHotel adminAttraction)
        {
            // Ensure the transpo ID is set
            if (adminAttraction.Id == 0)
            {
                return BadRequest(new { success = false, message = "Invalid attraction ID" });
            }
            ModelState.Remove(nameof(adminAttraction.Image1));
            ModelState.Remove(nameof(adminAttraction.Image2));
            ModelState.Remove(nameof(adminAttraction.Image3));
            ModelState.Remove(nameof(adminAttraction.Image4));
            ModelState.Remove(nameof(adminAttraction.Image5));
            ModelState.Remove(nameof(adminAttraction.Img1));
            ModelState.Remove(nameof(adminAttraction.Img2));
            ModelState.Remove(nameof(adminAttraction.Img3));
            ModelState.Remove(nameof(adminAttraction.Img4));
            ModelState.Remove(nameof(adminAttraction.Img5));
            if (!ModelState.IsValid)
            {
                var friendlyFieldNames = new Dictionary<string, string>
                {
                    { "Img1", "Primary Image" },
                    { "Img2", "Secondary Image 1" },
                    { "Img3", "Secondary Image 2" },
                    { "Img4", "Secondary Image 3" },
                    { "Img5", "Secondary Image 4" },
                    { "Name", "Place Name" },
                    { "ContactNumber", "Contact Number" },
                    { "LocationName", "Location Name" },
                    { "Price", "Attraction Price" },
                    { "GoogleMapsEmbed", "Google Maps Embed Link" },
                    { "Information", "Attraction Information" }
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
            // Find the existing attraction in the database
            var existingAttraction = await _context.AdminAttraction.FindAsync(adminAttraction.Id);
            if (existingAttraction == null)
            {
                return NotFound(new { success = false, message = "Attraction not found" });
            }

            // Update the attraction properties
            existingAttraction.Name = adminAttraction.Name;
            existingAttraction.ContactNumber = adminAttraction.ContactNumber;
            existingAttraction.LocationName = adminAttraction.LocationName;
            existingAttraction.Price = adminAttraction.Price;
            existingAttraction.GoogleMapsEmbed = adminAttraction.GoogleMapsEmbed;
            existingAttraction.Information = adminAttraction.Information;

            // Handle image updates (if any)
            var imageFields = new (IFormFile img, string imageUrlField)[]
            {
        (adminAttraction.Img1, "Image1"),
        (adminAttraction.Img2, "Image2"),
        (adminAttraction.Img3, "Image3"),
        (adminAttraction.Img4, "Image4"),
        (adminAttraction.Img5, "Image5")
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

                    var propertyInfo = existingAttraction.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(existingAttraction, "/docs/" + fileName);
                    }
                }
            }

            // Save the changes
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Attraction updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the attraction by id
            var attraction = await _context.AdminAttraction
                .Where(h => h.Id == id && h.Status == 1) // Ensure status is active (1)
                .FirstOrDefaultAsync();

            // Check if the attraction exists and has a status of 1 (active)
            if (attraction == null)
            {
                return NotFound(new { success = false, message = "Attraction not found or already deleted." });
            }

            // Update the status to 0 (deleted)
            attraction.Status = 0;

            // Save changes
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Attraction has been marked as deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error occurred: {ex.Message}" });
            }
        }

    }





}
