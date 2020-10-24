using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.Services.Interfaces;

namespace OnlineAuction.Services.Defenitions
{
    public class DeleteService: IDeleteService
    {
        //добавить контекс
        public async Task DeleteLotAsync(int id, ApplicationContext context)
        {
            var lot = await context.Lots.FindAsync(id);
            context.Lots.Remove(lot);

            await context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int lotId, int commentId, ApplicationContext context)
        {
            var comment = await context.Comments.FindAsync(commentId);
            
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }
    }
}