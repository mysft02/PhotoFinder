using System.ComponentModel.DataAnnotations;

namespace PhotoFinder.DTO.Notification
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    // DTO for creating a Notification
    public class CreateNotificationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Message { get; set; }

        public bool? IsRead { get; set; }
    }

    // DTO for updating a Notification
    public class UpdateNotificationDTO
    {
        [Required]
        public int NotificationId { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
    }

}
