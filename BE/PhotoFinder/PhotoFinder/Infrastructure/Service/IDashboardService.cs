using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.Entity;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IDashboardService
    {
        public Task<IActionResult> HandleGetUserBookingAnalytics(int userId);
        public Task<IActionResult> HandleGetPhotographerBookingAnalytics(int userId);
    }

    public class DashboardService : ControllerBase, IDashboardService
    {
        private readonly PhotoFinderContext _context;

        public DashboardService(PhotoFinderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetUserBookingAnalytics(int userId)
        {
            var bookingQuery = _context.Bookings.AsQueryable();

            var bookingAna = bookingQuery
                .Where(x => x.CustomerId == userId)
                .GroupBy(x => (x.UpdatedAt).Value.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            var incomeAna = bookingQuery
                .Where(x => x.CustomerId == userId)
                .GroupBy(x => (x.UpdatedAt).Value.Month)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalPrice));

            var result = new AnalyticDTO { bookingAna = bookingAna, incomeAna = incomeAna };

            return Ok(result);
        }

        public async Task<IActionResult> HandleGetPhotographerBookingAnalytics(int userId)
        {
            var bookingQuery = _context.Bookings.AsQueryable();

            var bookingAna = bookingQuery
                .Where(x => x.PhotographerId == userId)
                .GroupBy(x => (x.UpdatedAt).Value.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            var incomeAna = bookingQuery
                .Where(x => x.PhotographerId == userId)
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
