using System.Collections.Generic;

namespace OnlineAuction.Models
{
    public class Category
    {
        public int Id { get; set; }    
        public string Name { get; set; }

        public IList<Lot> Lots { get; set; }
    }
}