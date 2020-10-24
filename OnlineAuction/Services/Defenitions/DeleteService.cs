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
        private ApplicationContext _context;
        public async Task DeleteLotAsync(int id, ApplicationContext context)
        {
            var lot = await _context.Lots.FindAsync(id);
            _context.Lots.Remove(lot);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int lotId, int commentId, ApplicationContext context)
        {
            var comment = await context.Comments.FindAsync(commentId);
            
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }
    }
}