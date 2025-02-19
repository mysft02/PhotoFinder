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
    }
}
