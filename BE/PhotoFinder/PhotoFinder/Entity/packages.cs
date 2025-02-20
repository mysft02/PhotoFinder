namespace PhotoFinder.Entity
{
    public class packages
    {
        public int package_id { get; set; }
        public int photographer_id { get; set; }
        public string package_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int duration { get; set; }
        public DateTime created_at { get; set; }
        public photographers Photographer { get; set; }
        public List<bookings> bookings { get; set; }
        public List<photos> photos { get; set; }
    }
}
