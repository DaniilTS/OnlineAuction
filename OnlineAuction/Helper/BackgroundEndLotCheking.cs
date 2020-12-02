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

        public async Task ChekLot(int lotId)
        {
            var context = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
            var userManager = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();

            Lot lot = await context.Lots.FindAsync(lotId);
            EmailService emailService = new EmailService();

            string message;
            User user;
            if (string.IsNullOrEmpty(lot.WiningUserName))
            {
                user = await userManager.FindByIdAsync(lot.UserId);
                message = $"{user.UserName}, никто не купил ваш лот: {lot.Name}";
            }
            else
            {
                user = await userManager.FindByNameAsync(lot.WiningUserName);
                message = $"{user.UserName}, вы успешно приобрели лот: {lot.Name}";
            }
            
            await emailService.SendEmailAsync(user.Email, "OnlineAuction", message);
        }
    }
}