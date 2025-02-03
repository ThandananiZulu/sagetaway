using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sagetaway.Models
{
    public class AdminTranspo
    {
        [Key]
        public int Id { get; set; }


        [NotMapped]
        [Required]
        public IFormFile Img1 { get; set; }
        [StringLength(2048)]
        public string Image1 { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Img2 { get; set; }
        [StringLength(2048)]
        public string Image2 { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Img3 { get; set; }
        [StringLength(2048)]
        public string Image3 { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Img4 { get; set; }
        [StringLength(2048)]
        public string Image4 { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Img5 { get; set; }
        [StringLength(2048)]
        public string Image5 { get; set; }

        // Required name of the place.
        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        // Required contact number.
        [Required]
        [StringLength(100)]
        public string ContactNumber { get; set; }

        // Required location name.
        [Required]
        [StringLength(512)]
        public string LocationName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        // Required Google Maps embed link.
        [Required]
        public string GoogleMapsEmbed { get; set; }

        // Required detailed information or description of the hotel.
        [Required]
        public string Information { get; set; }


        [Required]
        [Range(0, 3)] // Ensure the value is either 0 or 1
        public int Status { get; set; } = 1;
    }
}
