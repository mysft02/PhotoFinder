﻿using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Entity;


namespace PhotoFinder.Infrastructure.Service
{
    public interface IPhotographerService
    {
        //Task<IActionResult> HandleCreatePhotographer(PhotographerCreateDTO photographerCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllPhotographers();
        Task<IActionResult> HandleGetPhotographerById(int id);
        Task<IActionResult> HandleUpdatePhotographer(PhotographerUpdateDTO photographerUpdateDTO, string? userId);
    }

    public class PhotographerService : ControllerBase, IPhotographerService
    {
        private readonly PhotoFinderContext _context;

        public PhotographerService(PhotoFinderContext context)
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
                        PhotographerId = x.PhotographerId,
                        UserId = x.UserId,
                        Bio = x.Bio,
                        PortfolioUrl = x.PortfolioUrl,
                        Rating = (double)x.Rating,
                        Location = x.Location,
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
                        PhotographerId = x.PhotographerId,
                        UserId = x.UserId,
                        Bio = x.Bio,
                        PortfolioUrl = x.PortfolioUrl,
                        Rating = (double)x.Rating,
                        Location = x.Location,
                    })
                    .FirstOrDefault(x => x.PhotographerId == id);

                return Ok(photographer);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        //public async Task<IActionResult> HandleCreatePhotographer(PhotographerCreateDTO photographerCreateDTO, string? userId)
        //{
        //    try
        //    {
        //        var photographer = new photographers
        //        {
        //            user_id = photographerCreateDTO.UserId,
        //            bio = photographerCreateDTO.Bio,
        //            portfolio_url = photographerCreateDTO.PortfolioUrl,
        //            location = photographerCreateDTO.Location,
        //            created_at = DateTime.Now
        //        };

        //        var user = _context.Users.FirstOrDefault(x => x.user_id == photographerCreateDTO.UserId);

        //        if(user.role != "photographer") { return BadRequest("User is not photographer"); }

        //        _context.Photographers.Add(photographer);
        //        var result = _context.SaveChanges();
        //        if (result > 0)
        //        {
        //            return Ok(photographer);
        //        }
        //        else
        //        {
        //            return BadRequest("Create failed");
        //        }
        //    }
        //    catch (Exception ex) { return BadRequest(ex.Message); }
        //}

        public async Task<IActionResult> HandleUpdatePhotographer(PhotographerUpdateDTO photographerUpdateDTO, string? userId)
        {
            try
            {
                var photographerQuery = _context.Photographers.AsQueryable();

                var photographer = photographerQuery.FirstOrDefault(x => x.PhotographerId == photographerUpdateDTO.PhotographerId);

                photographer.Bio = photographerUpdateDTO.Bio;
                photographer.PortfolioUrl = photographerUpdateDTO.PortfolioUrl;
                photographer.Rating = photographerUpdateDTO.Rating;
                photographer.Location = photographerUpdateDTO.Location;

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
