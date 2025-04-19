using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Package;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Entity;
using PhotoFinder.Infrastructure;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IPackageService
    {
        Task<IActionResult> HandleCreatePackage(PackageCreateDTO packageCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllPackages();
        Task<IActionResult> HandleGetPackageById(int id);
        Task<IActionResult> HandleUpdatePackage(PackageUpdateDTO packageUpdateDTO, string? userId);
        Task<IActionResult> HandleGetPackageByPhotographerId(int photographerId);
    }

    public class PackageService : ControllerBase, IPackageService
    {
        private readonly PhotoFinderContext _context;

        public PackageService(PhotoFinderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleGetAllPackages()
        {
            try
            {
                List<PackageDTO> packages = new List<PackageDTO>();
                packages = _context.Packages
                    .Select(x => new PackageDTO
                    {
                        PackageId = x.PackageId,
                        PhotographerId = x.PhotographerId,
                        PackageName = x.PackageName,
                        Description = x.Description,
                        Price = x.Price,
                        CreatedAt = x.CreatedAt,
                        Duration = x.Duration,
                    })
                    .ToList();

                return Ok(packages);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetPackageById(int id)
        {
            try
            {
                var package = _context.Packages
                    .Select(x => new PackageDTO
                    {
                        PackageId = x.PackageId,
                        PhotographerId = x.PhotographerId,
                        PackageName = x.PackageName,
                        Description = x.Description,
                        Price = x.Price,
                        CreatedAt = x.CreatedAt,
                        Duration = x.Duration,
                    })
                    .FirstOrDefault(x => x.PackageId == id);

                return Ok(package);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreatePackage(PackageCreateDTO packageCreateDTO, string? userId)
        {
            try
            {
                var package = new Package
                {
                    PhotographerId = packageCreateDTO.PhotographerId,
                    PackageName = packageCreateDTO.PackageName,
                    Description = packageCreateDTO.Description,
                    Price = packageCreateDTO.Price,
                    CreatedAt = DateTime.Now,
                    Duration = packageCreateDTO.Duration
                };

                _context.Packages.Add(package);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(package);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdatePackage(PackageUpdateDTO packageUpdateDTO, string? userId)
        {
            try
            {
                var packageQuery = _context.Packages.AsQueryable();

                var package = packageQuery.FirstOrDefault(x => x.PackageId == packageUpdateDTO.PackageId);

                package.PhotographerId = packageUpdateDTO.PhotographerId;
                package.PackageName = packageUpdateDTO.PackageName;
                package.Description = packageUpdateDTO.Description;
                package.Price = packageUpdateDTO.Price;
                package.Duration = packageUpdateDTO.Duration;

                _context.Packages.Update(package);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(package);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetPackageByPhotographerId(int photographerId)
        {
            try
            {
                List<PackageDTO> packages = _context.Packages
         .Where(x => x.PhotographerId == photographerId) // Filter packages by photographerId first
         .Select(x => new PackageDTO
         {
             PackageId = x.PackageId,
             PhotographerId = x.PhotographerId,
             PackageName = x.PackageName,
             Description = x.Description,
             Price = x.Price,
             CreatedAt = x.CreatedAt,
             Duration = x.Duration,
         })
         .ToList(); // Then project to DTO and convert to a list

                return Ok(packages);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
