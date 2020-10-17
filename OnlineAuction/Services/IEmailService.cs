using System.Threading.Tasks;

namespace OnlineAuction
{
    public interface IEmailService
    { 
        Task SendEmailAsync(string email, string subject, string message);
    }
}