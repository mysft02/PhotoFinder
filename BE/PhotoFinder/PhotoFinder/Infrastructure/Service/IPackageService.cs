using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Package;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Entity;
using PhotoFinder.Infrastructure.Database;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IPackageService
    {
        Task<IActionResult> HandleCreatePackage(PackageCreateDTO packageCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllPackages();
        Task<IActionResult> HandleGetPackageById(int id);
        Task<IActionResult> HandleUpdatePackage(PackageUpdateDTO packageUpdateDTO, string? userId);
    }

    public class PackageService : ControllerBase, IPackageService
    {
        private readonly PhotoFinderDbContext _context;

        public PackageService(PhotoFinderDbContext context)
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
                        package_id = x.package_id,
                        photographer_id = x.photographer_id,
                        package_name = x.package_name,
                        description = x.description,
                        price = x.price,
                        created_at = x.created_at,
                        duration = x.duration,
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
                        package_id = x.package_id,
                        photographer_id = x.photographer_id,
                        package_name = x.package_name,
                        description = x.description,
                        price = x.price,
                        created_at = x.created_at,
                        duration = x.duration,
                    })
                    .FirstOrDefault(x => x.package_id == id);

                return Ok(package);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleCreatePackage(PackageCreateDTO packageCreateDTO, string? userId)
        {
            try
            {
                var package = new packages
                {
                    photographer_id = packageCreateDTO.photographer_id,
                    package_name = packageCreateDTO.package_name,
                    description = packageCreateDTO.description,
                    price = packageCreateDTO.price,
                    created_at = DateTime.Now,
                    duration = packageCreateDTO.duration
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

                var package = packageQuery.FirstOrDefault(x => x.package_id == packageUpdateDTO.package_id);

                package.photographer_id = packageUpdateDTO.photographer_id;
                package.package_name = packageUpdateDTO.package_name;
                package.description = packageUpdateDTO.description;
                package.price = packageUpdateDTO.price;
                package.duration = packageUpdateDTO.duration;

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
    }
}
