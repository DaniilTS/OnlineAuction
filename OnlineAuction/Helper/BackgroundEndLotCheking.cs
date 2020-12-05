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
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public BackgroundEndLotCheking(IServiceProvider serviceProvider,UserManager<User> userManager,
            ApplicationContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _userManager = userManager;
        }

        public async Task ChekLot(int lotId)
        {
            Lot lot = await _context.Lots.FindAsync(lotId);
            EmailService emailService = new EmailService();

            string message;
            User user;
            if (string.IsNullOrEmpty(lot.WiningUserName))
            {
                user = await _userManager.FindByIdAsync(lot.UserId);
                message = $"{user.UserName}, никто не купил ваш лот: {lot.Name}";
            }
            else
            {
                user = await _userManager.FindByNameAsync(lot.WiningUserName);
                message = $"{user.UserName}, вы успешно приобрели лот: {lot.Name}";
            }
            
            await emailService.SendEmailAsync(user.Email, "OnlineAuction", message);
        }
    }
}