using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}