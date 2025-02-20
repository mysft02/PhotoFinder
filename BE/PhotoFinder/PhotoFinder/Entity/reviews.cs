namespace PhotoFinder.Entity
{
    public class reviews
    {
        public int review_id { get; set; }
        public int booking_id { get; set; }
        public double rating { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
        public bookings Booking { get; set; }
    }
}
