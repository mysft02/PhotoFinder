namespace PhotoFinder.Entity
{
    public class availability
    {
        public int availability_id { get; set; }
        public int photographer_id { get; set; }
        public DateTime date { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public photographers Photographer { get; set; }
    }
}
