using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string UserName { get; set; }
         
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
         
        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
         
        public string ReturnUrl { get; set; }    
    }
}