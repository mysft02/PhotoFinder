using System.ComponentModel.DataAnnotations;

namespace PhotoFinder.DTO.Booking
{
    public class BookingDTO
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int PhotographerId { get; set; }

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; }

        public string Status { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }

  
public class CreateBookingDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PhotographerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
       
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Event location must be between 5 and 255 characters.")]
        public string EventLocation { get; set; }

        [Required]
        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$", ErrorMessage = "Invalid status.")]
        public string Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a non-negative value.")]
        public decimal TotalPrice { get; set; }
    }

    public class UpdateBookingDTO
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public int? PhotographerId { get; set; }

        [DataType(DataType.Date)]
     
        public DateTime? EventDate { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "Event location must be between 5 and 255 characters.")]
        public string? EventLocation { get; set; }

        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$", ErrorMessage = "Invalid status.")]
        public string? Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a non-negative value.")]
        public decimal? TotalPrice { get; set; }
    }


}
