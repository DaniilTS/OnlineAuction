using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuction.Data;
using OnlineAuction.Services.Interfaces;
using OnlineAuction.ViewModels;

namespace OnlineAuction.Services.Defenitions
{
    public class LotService : ILotService
    {
        private ApplicationContext _context;
        public LotService(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
        }

        public async Task UpdateLotPost(int id, EditLotViewModel model)
        {
            
            var lot = await _context.Lots.FindAsync(model.Id);

            lot.Name = model.Name;
            lot.Description = model.Description;
            lot.StartCurrency = model.StartCurrency;
            lot.CategoryId = model.CategoryId;
            lot.Category = await _context.Categories.FindAsync(lot.CategoryId);
            lot.PublicationDate = model.PublicationDate.ToUniversalTime();
            lot.FinishDate = model.FinishDate.ToUniversalTime();

            _context.Lots.Update(lot);
            await _context.SaveChangesAsync();
        }
    }
}