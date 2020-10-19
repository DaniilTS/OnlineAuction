using System.Threading.Tasks;
using OnlineAuction.Data;
using OnlineAuction.ViewModels;

namespace OnlineAuction.Services.Interfaces
{
    public interface ILotService
    {
        Task UpdateLotPost(int id, EditLotViewModel model, ApplicationContext context);
    }
}