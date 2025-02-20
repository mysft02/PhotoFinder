namespace PhotoFinder.Entity
{
    public class notifications 
    {
        public int notification_id { get; set; }
        public int user_id { get; set; }
        public string message { get; set; }
        public bool is_read { get; set; }
        public DateTime created_at { get; set; }
        public users user { get; set; }
    }
}
