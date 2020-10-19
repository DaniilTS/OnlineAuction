using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.Services.Interfaces;

namespace OnlineAuction.Services.Defenitions
{
    public class DeleteService: IDeleteService
    {
        public async Task DeleteLot(int id, ApplicationContext context)
        {
            var lot = await context.Lots.FindAsync(id);
            context.Lots.Remove(lot);

            await context.SaveChangesAsync();
        } 
    }
}