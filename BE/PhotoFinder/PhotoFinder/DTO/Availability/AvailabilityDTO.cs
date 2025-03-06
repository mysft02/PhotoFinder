using PhotoFinder.Infrastructure.Service;
using System.Text.Json.Serialization;

namespace PhotoFinder.DTO.Availability
{
    public class AvailabilityDTO
    {
        public int AvailabilityId { get; set; }

        public int PhotographerId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }

    public class AvailabilityCreateDTO 
    {
        public int PhotographerId { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }  

    public class AvailabilityUpdateDTO
    {
        public int AvailabilityId { get; set; }
        public int PhotographerId { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
