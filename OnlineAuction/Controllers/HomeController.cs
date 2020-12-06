using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineAuction.Data;
using OnlineAuction.Helper;
using OnlineAuction.Models;
using OnlineAuction.Services.Interfaces;
using OnlineAuction.ViewModels;
using Org.BouncyCastle.Asn1.Cms;

namespace OnlineAuction.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        private readonly ILotService _lotService;
        private readonly IDeleteService _deleteService;

        public HomeController(UserManager<User> userManager,
            ApplicationContext context,
            ILotService lotService,
            IDeleteService deleteService)
        {
            _context = context;
            _userManager = userManager;
            _lotService = lotService;
            _deleteService = deleteService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Lots
                .Include(u => u.User)
                .Include(c => c.Category)
                .ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [Authorize]
        public async Task<IActionResult> CreateLot()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            ViewData["CurrentUserTime"] = DateTime.UtcNow.AddHours(int.Parse(user.TimeZone));
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateLot(CreateLotViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var hours = int.Parse(user.TimeZone);

            if (ModelState.IsValid
                && model.FinishDate > model.PublicationDate
                && model.PublicationDate > DateTime.UtcNow.AddHours(hours)
                && model.FinishDate > DateTime.UtcNow.AddHours(hours))
            {
                var lot = new Lot
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Category = await _context.Categories.FindAsync(model.CategoryId),
                    Description = model.Description,
                    StartCurrency = model.StartCurrency,
                    User = user,
                    PublicationDate = model.PublicationDate.AddHours(-1*hours),
                    FinishDate = model.FinishDate.AddHours(-1*hours),
                    IsEmailSended = false
                };
                
                await _context.Lots.AddAsync(lot);
                await _context.SaveChangesAsync();
                
                double diff = (model.FinishDate - DateTime.UtcNow.AddHours(hours)).TotalMinutes + 1;
                BackgroundJob.Schedule<BackgroundEndLotCheking>(x => x.ChekLot(lot.Id),
                    TimeSpan.FromMinutes(diff));

                return RedirectToAction("Index");
            }

            return RedirectToAction("CreateLot");
        }

        [Authorize]
        public async Task<IActionResult> DeleteLot(int id)
        {
            await _deleteService.DeleteLotAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditLot(int id)
        {
            var lot = await _context.Lots.FindAsync(id);
            if (lot == null)
            {
                return NotFound();
            }

            EditLotViewModel model = new EditLotViewModel
            {
                Name = lot.Name,
                Description = lot.Description,
                StartCurrency = lot.StartCurrency,
                CategoryId = lot.CategoryId,
                Category = await _context.Categories.FindAsync(lot.CategoryId),
                PublicationDate = lot.PublicationDate,
                FinishDate = lot.FinishDate
            };

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLot(EditLotViewModel model)
        {
            if (ModelState.IsValid && (model.FinishDate > model.PublicationDate))
            {
                await _lotService.UpdateLotPost(model.Id, model);
                return RedirectToAction("Details", new {id = model.Id});
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Лот не найден");
            }

            return View(model);
        }

        public async Task<IActionResult> RaisePrice(int id, string userName)
        {
            var lot = await _context.Lots.FindAsync(id);
            lot.StartCurrency += 50;

            var user = await _userManager.FindByNameAsync(userName);
            lot.WiningUserId = user.Id;
            lot.WiningUserName = user.UserName;

            _context.Lots.Update(lot);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = id});
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = _context.Lots.Where(i => i.Id == id)
                .Include(u => u.User)
                .Include(c => c.Category)
                .Include(com => com.Comments)
                .ThenInclude(i => i.User)
                .First();

            if (lot == null)
            {
                return NotFound();
            }

            LotViewModel model;
            if (User.Identity.IsAuthenticated)
            {
                model = new LotViewModel
                {
                    Lot = lot,
                    CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)
                };
            }
            else
            {
                model = new LotViewModel
                {
                    Lot = lot,
                };
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> WriteComment(int lotId, CommentInput commentInput)
        {
            string userName = HttpContext.User.Identity.Name;
            var lot = await _context.Lots.FindAsync(lotId);
            var user = await _userManager.FindByNameAsync(userName);

            Comment comment = new Comment
            {
                UserId = user.Id,
                User = user,
                LotId = lotId,
                Lot = lot,
                Text = commentInput.Comment
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = lotId});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int lotId, int commentId)
        {
            await _deleteService.DeleteCommentAsync(lotId, commentId);

            return RedirectToAction("Details", new {id = lotId});
        }
    }
}