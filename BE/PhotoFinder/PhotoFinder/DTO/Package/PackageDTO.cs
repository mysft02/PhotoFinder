namespace PhotoFinder.DTO.Package
{
    public class PackageDTO
    {
        public int PackageId { get; set; }

        public int PhotographerId { get; set; }

        public string PackageName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class PackageCreateDTO
    {
        public int PhotographerId { get; set; }

        public string PackageName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }
    }

    public class PackageUpdateDTO
    {
        public int PackageId { get; set; }

        public int PhotographerId { get; set; }

        public string PackageName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }
    }
}
