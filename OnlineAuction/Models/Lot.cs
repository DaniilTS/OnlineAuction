using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string WiningUserId { get; set; }
        
        public decimal StartCurrency { get; set; }

        public string Name { get; set;}
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IList<Comment> Comments { get; set; }

        [DataType(DataType.DateTime)] 
        public DataType PublicationDate { get; set; }
    }
}