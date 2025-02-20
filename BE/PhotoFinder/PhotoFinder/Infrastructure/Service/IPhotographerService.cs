using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Entity;
using PhotoFinder.Infrastructure.Database;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IPhotographerService
    {
        Task<IActionResult> HandleCreatePhotographer(PhotographerCreateDTO photographerCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllPhotographers();
        Task<IActionResult> HandleGetPhotographerById(int id);
        Task<IActionResult> HandleUpdatePhotographer(PhotographerUpdateDTO photographerUpdateDTO, string? userId);
    }

    public class PhotographerService : ControllerBase, IPhotographerService
    {
        private readonly PhotoFinderDbContext _context;

        public PhotographerService(PhotoFinderDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllPhotographers()
        {
            try
            {
                List<PhotographerDTO> photographers = new List<PhotographerDTO>();
                photographers = _context.Photographers
                    .Select(x => new PhotographerDTO{
                        PhotographerId = x.photographer_id,
                        UserId = x.user_id,
                        Bio = x.bio,
                        PortfolioUrl = x.portfolio_url,
                        Rating = x.rating,
                        Location = x.location,
                    })
                    .ToList();
                
                return Ok(photographers);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetPhotographerById(int id)
        {
            try
            {
                var photographer = _context.Photographers
                    .Select(x => new PhotographerDTO
                    {
                        PhotographerId = x.photographer_id,
                        UserId = x.user_id,
                        Bio = x.bio,
                        PortfolioUrl = x.portfolio_url,
                        Rating = x.rating,
                        Location = x.location,
                    })
                    .FirstOrDefault(x => x.PhotographerId == id);

                return Ok(photographer);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreatePhotographer(PhotographerCreateDTO photographerCreateDTO, string? userId)
        {
            try
            {
                var photographer = new photographers
                {
                    user_id = photographerCreateDTO.UserId,
                    bio = photographerCreateDTO.Bio,
                    portfolio_url = photographerCreateDTO.PortfolioUrl,
                    location = photographerCreateDTO.Location,
                    created_at = DateTime.Now
                };

                _context.Photographers.Add(photographer);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(photographer);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdatePhotographer(PhotographerUpdateDTO photographerUpdateDTO, string? userId)
        {
            try
            {
                var photographerQuery = _context.Photographers.AsQueryable();

                var photographer = photographerQuery.FirstOrDefault(x => x.photographer_id == photographerUpdateDTO.PhotographerId);

                photographer.bio = photographerUpdateDTO.Bio;
                photographer.portfolio_url = photographerUpdateDTO.PortfolioUrl;
                photographer.rating = photographerUpdateDTO.Rating;
                photographer.location = photographerUpdateDTO.Location;

                _context.Photographers.Update(photographer);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(photographer);
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
