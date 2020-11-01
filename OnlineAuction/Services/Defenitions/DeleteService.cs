using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.Services.Interfaces;

namespace OnlineAuction.Services.Defenitions
{
    public class DeleteService: IDeleteService
    {
        private ApplicationContext _context;

        public DeleteService(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        public async Task DeleteLotAsync(int id)
        {
            var lot = await _context.Lots.FindAsync(id);
            _context.Lots.Remove(lot);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int lotId, int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}