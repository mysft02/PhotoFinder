namespace PhotoFinder.Entity
{
    public class photos
    {
        public int photo_id { get; set; }
        public int photographer_id { get; set; }
        public int package_id { get; set; }
        public int booking_id { get; set; }
        public string photo_url { get; set; }
        public bool is_public { get; set; }
        public DateTime uploaded_at { get; set; }
        public photographers photographer { get; set; }
        public packages package { get; set; }
        public bookings booking { get; set; }
    }
}
