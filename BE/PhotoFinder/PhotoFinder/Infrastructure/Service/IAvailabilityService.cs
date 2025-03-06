using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Availability;
using PhotoFinder.DTO.Package;
using PhotoFinder.Entity;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IAvailabilityService
    {
        Task<IActionResult> HandleCreateAvailability(AvailabilityCreateDTO availabilityCreateDTO, string? userId);
        Task<IActionResult> HandleGetAvailabilityByPhotographerId(string? userId);
        Task<IActionResult> HandleUpdateAvailability(AvailabilityUpdateDTO availabilityUpdateDTO, string? userId);
    }

    public class AvailabilityService : ControllerBase, IAvailabilityService
    {
        private readonly PhotoFinderContext _context;

        public AvailabilityService(PhotoFinderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleCreateAvailability(AvailabilityCreateDTO availabilityCreateDTO, string? userId)
        {
            try
            {
                var avai = new Availability
                {
                    PhotographerId = availabilityCreateDTO.PhotographerId,
                    Date = DateOnly.FromDateTime(availabilityCreateDTO.Date),
                    StartTime = TimeOnly.FromDateTime(availabilityCreateDTO.StartTime),
                    EndTime = TimeOnly.FromDateTime(availabilityCreateDTO.EndTime)
                };

                _context.Availabilities.Add(avai);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(availabilityCreateDTO);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetAvailabilityByPhotographerId(string? userId)
        {
            try
            {
                var availability = _context.Availabilities
                    .Where(x => x.PhotographerId.ToString() == userId)
                    .ToList();

                return Ok(availability);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateAvailability(AvailabilityUpdateDTO availabilityUpdateDTO, string? userId)
        {
            try
            {
                var availabilityQuery = _context.Availabilities.AsQueryable();

                var availability = availabilityQuery.FirstOrDefault(x => x.AvailabilityId == availabilityUpdateDTO.AvailabilityId);
                availability.Date = DateOnly.FromDateTime(availabilityUpdateDTO.Date);
                availability.StartTime = TimeOnly.FromDateTime(availabilityUpdateDTO.StartTime);
                availability.EndTime = TimeOnly.FromDateTime(availabilityUpdateDTO.StartTime);

                _context.Availabilities.Update(availability);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(availability);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}