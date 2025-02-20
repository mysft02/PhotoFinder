namespace PhotoFinder.DTO.Package
{
    public class PackageDTO
    {
        public int package_id { get; set; }
        public int photographer_id { get; set; }
        public string package_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int duration { get; set; }
        public DateTime created_at { get; set; }
    }

    public class PackageCreateDTO
    {
        public int photographer_id { get; set; }
        public string package_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int duration { get; set; }
    }

    public class PackageUpdateDTO
    {
        public int package_id { get; set; }
        public int photographer_id { get; set; }
        public string package_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int duration { get; set; }
    }
}
