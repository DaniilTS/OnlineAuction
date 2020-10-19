using System.Threading.Tasks;

namespace OnlineAuction.Services.Interfaces
{
    public interface IEmailService
    { 
        Task SendEmailAsync(string email, string subject, string message);
        
    }
}