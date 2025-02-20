namespace PhotoFinder.Entity
{
    public class users : BaseEntities
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string phone_number { get; set; }
        public string profile_picture { get; set; }
        public List<bookings> bookings { get; set; }
        public List<notifications> notifications { get; set; }
        public List<conversations> conversations { get; set; }
        public List<messages> messages { get; set; }
    }
}
