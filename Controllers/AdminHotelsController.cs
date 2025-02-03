using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using sagetaway.Models;
using Sagetaway.Data;    // Assuming AdminHotel is in this namespace
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace sagetaway.Controllers
{

    public class AdminHotelsController : Controller
    {
        private readonly SagetawayContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminHotelsController(SagetawayContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var hotels = _context.AdminHotels
                      .Where(h => h.Status == 1)
                      .ToList();

            return Json(new { data = hotels });
        }

        // POST: api/AdminHotels/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdminHotel adminHotel)
        {
            ModelState.Remove(nameof(adminHotel.Image1));
            ModelState.Remove(nameof(adminHotel.Image2));
            ModelState.Remove(nameof(adminHotel.Image3));
            ModelState.Remove(nameof(adminHotel.Image4));
            ModelState.Remove(nameof(adminHotel.Image5));
            if (!ModelState.IsValid)
            {
                var friendlyFieldNames = new Dictionary<string, string>
                {
                    { "Img1", "Primary Image" },
                    { "Img2", "Secondary Image 1" },
                    { "Img3", "Secondary Image 2" },
                    { "Img4", "Secondary Image 3" },
                    { "Img5", "Secondary Image 4" },
                    { "NameOfPlace", "Place Name" },
                    { "ContactNumber", "Contact Number" },
                    { "LocationName", "Location Name" },
                    { "Price", "Hotel Price" },
                    { "GoogleMapsEmbed", "Google Maps Embed Link" },
                    { "HotelInformation", "Hotel Information" }
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
            (adminHotel.Img1, "Image1"),
            (adminHotel.Img2, "Image2"),
            (adminHotel.Img3, "Image3"),
            (adminHotel.Img4, "Image4"),
            (adminHotel.Img5, "Image5")
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
                    var propertyInfo = adminHotel.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        // Set the URL dynamically using reflection
                        propertyInfo.SetValue(adminHotel, "/docs/" + fileName);
                    }
                }
            }
            try
            {
                // Add the AdminHotel entity to the DbContext
                _context.AdminHotels.Add(adminHotel);

                // Save changes to the database
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok(new { success = true, message = "Hotel added successfully!" });
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
        public async Task<IActionResult> Update([FromForm] AdminHotel adminHotel)
        {
            // Ensure the hotel ID is set
            if (adminHotel.Id == 0)
            {
                return BadRequest(new { success = false, message = "Invalid hotel ID" });
            }

            // Find the existing hotel in the database
            var existingHotel = await _context.AdminHotels.FindAsync(adminHotel.Id);
            if (existingHotel == null)
            {
                return NotFound(new { success = false, message = "Hotel not found" });
            }

            // Update the hotel properties
            existingHotel.NameOfPlace = adminHotel.NameOfPlace;
            existingHotel.ContactNumber = adminHotel.ContactNumber;
            existingHotel.LocationName = adminHotel.LocationName;
            existingHotel.Price = adminHotel.Price;
            existingHotel.GoogleMapsEmbed = adminHotel.GoogleMapsEmbed;
            existingHotel.HotelInformation = adminHotel.HotelInformation;

            // Handle image updates (if any)
            var imageFields = new (IFormFile img, string imageUrlField)[]
            {
        (adminHotel.Img1, "Image1"),
        (adminHotel.Img2, "Image2"),
        (adminHotel.Img3, "Image3"),
        (adminHotel.Img4, "Image4"),
        (adminHotel.Img5, "Image5")
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

                    var propertyInfo = existingHotel.GetType().GetProperty(imageUrlField);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(existingHotel, "/docs/" + fileName);
                    }
                }
            }

            // Save the changes
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Hotel updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the hotel by id
            var hotel = await _context.AdminHotels
                .Where(h => h.Id == id && h.Status == 1) // Ensure status is active (1)
                .FirstOrDefaultAsync();

            // Check if the hotel exists and has a status of 1 (active)
            if (hotel == null)
            {
                return NotFound(new { success = false, message = "Hotel not found or already deleted." });
            }

            // Update the status to 0 (deleted)
            hotel.Status = 0;

            // Save changes
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Hotel has been marked as deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error occurred: {ex.Message}" });
            }
        }

    }





}
