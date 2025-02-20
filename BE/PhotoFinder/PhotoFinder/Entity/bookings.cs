namespace PhotoFinder.Entity
{
    public class bookings : BaseEntities
    {
        public int booking_id { get; set; }
        public int customer_id { get; set; }
        public int photographer_id { get; set; }
        public int package_id { get; set; }
        public DateTime event_date { get; set; }
        public string event_location { get; set; }
        public decimal total_price { get; set; }
        public string status { get; set; }
        public users Customer { get; set; }
        public photographers Photographer { get; set; }
        public packages Package { get; set; }
        public reviews Review { get; set; }
        public List<photos> photos { get; set; }
    }
}
