using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using OnlineAuction.Services.Interfaces;

namespace OnlineAuction.Services.Defenitions
{
    public class EmailService: IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "d.tsukrov@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("d.tsukrov@gmail.com", "Danik200156");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}