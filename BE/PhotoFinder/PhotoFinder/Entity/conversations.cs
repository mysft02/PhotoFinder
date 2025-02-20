namespace PhotoFinder.Entity
{
    public class conversations : BaseEntities
    {
        public int conversation_id { get; set; }
        public int customer_id { get; set; }
        public int photographer_id { get; set; }
        public users customer { get; set; }
        public photographers photographer { get; set; }
        public List<messages> messages { get; set; }
    }
}
