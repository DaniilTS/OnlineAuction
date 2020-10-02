using Microsoft.AspNetCore.Identity;

namespace OnlineAuction.Models
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
    }
}