namespace PhotoFinder.Entity
{
    public class messages
    {
        public int message_id { get; set; }
        public int conversation_id { get; set; }
        public int sender_id { get; set; }
        public string content { get; set; }
        public bool is_read { get; set; }
        public DateTime sent_at { get; set; }
        public conversations conversations { get; set; }
        public users senders { get; set; }
    }
}
