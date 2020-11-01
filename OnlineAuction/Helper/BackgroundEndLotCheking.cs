using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.Services.Defenitions;

namespace OnlineAuction.Helper
{
    public class BackgroundEndLotCheking
    {
        readonly IServiceProvider _serviceProvider;

        public BackgroundEndLotCheking(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ChekLot()
        {
            var context = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
            var userManager = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
            
            var lots = await context.Lots
                .Where(d => (!d.IsEmailSended))
                .ToListAsync();
            
            EmailService emailService = new EmailService();
            foreach (var lot in lots)
            {
                if (lot.FinishDate < DateTime.UtcNow)
                {
                    if(lot.WiningUserName!=null)
                    {
                        var winingUser = await userManager.FindByNameAsync(lot.WiningUserName);
                        var message = $"{winingUser.UserName}, вы успешно приобрели лот: {lot.Name}";

                        await emailService.SendEmailAsync(winingUser.Email, "OnlineAuction", message);
                    }
                    else
                    {
                        var user = await userManager.FindByIdAsync(lot.UserId);
                        var message = $"{user.UserName}, никто не купил ваш лот: {lot.Name}";

                        await emailService.SendEmailAsync(user.Email, "OnlineAuction", message);
                    }

                    lot.IsEmailSended = true;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}