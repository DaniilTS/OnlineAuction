using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string UserId { get; set; }
        public int Year { get; set; }
    }
}