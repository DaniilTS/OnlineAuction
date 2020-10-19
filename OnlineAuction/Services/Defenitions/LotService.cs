using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineAuction.Services.Interfaces;

namespace OnlineAuction.Services
{
    public class LotService : ILotService
    {
        public async Task UpdateLotPost(int id, EditLotViewModel model, ApplicationContext context)
        {
            var lot = await context.Lots.FindAsync(model.Id);

            lot.Name = model.Name;
            lot.Description = model.Description;
            lot.StartCurrency = model.StartCurrency;
            lot.CategoryId = model.CategoryId;
            lot.Category = await context.Categories.FindAsync(lot.CategoryId);
            lot.PublicationDate = model.PublicationDate;
            lot.FinishDate = model.FinishDate;

            context.Lots.Update(lot);
            await context.SaveChangesAsync();
        }
    }
}