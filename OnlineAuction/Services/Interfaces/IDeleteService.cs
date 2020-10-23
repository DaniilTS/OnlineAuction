using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineAuction.Data;
using OnlineAuction.Models;

namespace OnlineAuction.Services.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteLotAsync(int id, ApplicationContext context);
        Task DeleteCommentAsync(int lotId, int commentId, ApplicationContext context);
    }
}