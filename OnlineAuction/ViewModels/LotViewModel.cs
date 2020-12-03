using System.Security.Claims;
using OnlineAuction.Models;

namespace OnlineAuction.ViewModels
{
    public class LotViewModel : CommentInput
    {
        public Lot Lot { get; set; }
        public User CurrentUser { get; set; }
    }
}