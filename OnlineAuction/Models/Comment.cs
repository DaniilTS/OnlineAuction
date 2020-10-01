namespace OnlineAuction.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public int LotId { get; set; }
        public Lot Lot { get; set; }
        
        public string Text { get; set; }
    }
}