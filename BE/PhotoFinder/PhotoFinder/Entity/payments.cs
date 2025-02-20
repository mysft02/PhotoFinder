namespace PhotoFinder.Entity
{
    public class payments
    {
        public int payment_id { get; set; }
        public int booking_id { get; set; }
        public string payment_method { get; set; }
        public string payment_status { get; set; }
        public DateTime payment_date { get; set; }
        public decimal amount { get; set; }
        public bookings Booking { get; set; }
    }
}
