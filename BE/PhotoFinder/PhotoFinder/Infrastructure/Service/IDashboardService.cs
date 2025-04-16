using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.Entity;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IDashboardService
    {
        public Task<IActionResult> HandleGetBooking();
    }

    public class DashboardService : ControllerBase, IDashboardService
    {
        private readonly PhotoFinderContext _context;

        public DashboardService(PhotoFinderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetBooking()
        {
            var bookingQuery = _context.Bookings.AsQueryable();

            var bookingAna = bookingQuery
                .GroupBy(x => (x.UpdatedAt).Value.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            var incomeAna = bookingQuery
                .GroupBy(x => (x.UpdatedAt).Value.Month)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalPrice));

            var result = new AnalyticDTO { bookingAna = bookingAna, incomeAna = incomeAna };

            return Ok(result);
        }

        public class AnalyticDTO
        {
            public Dictionary<int, int> bookingAna { get; set; }
            public Dictionary<int, decimal> incomeAna { get; set; }
        }
    }
}
