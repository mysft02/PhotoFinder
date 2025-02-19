namespace PhotoFinder.DTO.Photographer
{
    public class PhotographerDTO
    {
        public int PhotographerId { get; set; }
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string PortfolioUrl { get; set; }
        public double Rating { get; set; }
        public string Location { get; set; }
    }

    public class PhotographerCreateDTO
    {
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string PortfolioUrl { get; set; }
        public string Location { get; set; }
    }

    public class PhotographerUpdateDTO
    {
        public int PhotographerId { get; set; }
        public string Bio { get; set; }
        public string PortfolioUrl { get; set; }
        public double Rating { get; set; }
        public string Location { get; set; }
    }
}
