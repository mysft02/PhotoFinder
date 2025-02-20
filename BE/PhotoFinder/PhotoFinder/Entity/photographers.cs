namespace PhotoFinder.Entity
{
    public class photographers
    {
        public int photographer_id { get; set; }
        public int user_id { get; set; }
        public string bio { get; set; }
        public string portfolio_url { get; set; }
        public double rating { get; set; }
        public string location { get; set; }
        public users User { get; set; }
        public DateTime created_at { get; set; }
        public List<packages> packages { get; set; }
        public List<bookings> bookings { get; set; }
        public List<availability> availabilities { get; set; }
        public List<conversations> conversations { get; set; }
        public List<photos> photos { get; set; }
    }
}
