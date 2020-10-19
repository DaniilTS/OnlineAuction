using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuction.Data;
using OnlineAuction.Models;
using OnlineAuction.Services;
using OnlineAuction.Services.Interfaces;
using OnlineAuction.ViewModels;

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
                .Include(c=> c.Category)
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
        public IActionResult CreateLot()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(),"Id", "Name");
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateLot(CreateLotViewModel model)
        {
            if (ModelState.IsValid && (model.FinishDate > model.PublicationDate))
            {
                Lot lot = new Lot
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Category = await _context.Categories.FindAsync(model.CategoryId),
                    Description = model.Description,
                    StartCurrency = model.StartCurrency,
                    PublicationDate = model.PublicationDate,
                    FinishDate = model.FinishDate,
                    User = await _userManager.GetUserAsync(HttpContext.User)
                };

                await _context.Lots.AddAsync(lot);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("odd user", "Дата конца должна быть больше даты начала");
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> DeleteLot(int id)
        {
            await _deleteService.DeleteLot(id, _context);
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
            
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(),"Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLot(EditLotViewModel model)
        {
            if(ModelState.IsValid && (model.FinishDate > model.PublicationDate))
            {
                await _lotService.UpdateLotPost(model.Id, model, _context);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Лот не найден");
            }

            return View(model);
        }
    }
}