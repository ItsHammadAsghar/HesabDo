using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HesabDo.Data;
using HesabDo.Models;
using HesabDo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HesabDo.Controllers
{
    public class HesabAccountController : Controller
    {
        // GET: /<controller>/
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HesabAccountController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
                                        AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime dateTomorrow = dateTimeNow.Date.AddDays(1);
            var user = await userManager.GetUserAsync(HttpContext.User);
            var data = await context.HesabAccounts.Where(e => e.ID == id).FirstAsync();
            var exp = await context.Expenses.Where(e => e.ApplicationUser.Id == user.Id).Where(e => e.DateTime < dateTomorrow)
                                                .Include(e => e.HesabAccount).OrderByDescending(e => e.DateTime).ToListAsync();
            var dep = await context.Deposits.Where(e => e.ApplicationUser.Id == user.Id).Where(e => e.DateTime < dateTomorrow)
                                                .Include(e => e.HesabAccount)
                                                .OrderByDescending(e => e.DateTime).ToListAsync();
            HesabAccountDetailViewModel model = new HesabAccountDetailViewModel{
                HesabAccount = data,
                Deposits = dep,
                Expenses = exp
            };
            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var data = await context.HesabAccounts.ToListAsync();
            return View(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Join(int id)
        {
            var acc = await context.HesabAccounts.Where(c => c.ID == id).FirstAsync();
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (signInManager.IsSignedIn(User))
            {
                HesabAccountUser m = new HesabAccountUser
                {
                    UserID = user.Id,
                    User = user,
                    HesabAccountID = acc.ID,
                    HesabAccount = acc
                };
                await context.HesabAccountUsers.AddAsync(m);
                await context.SaveChangesAsync();

                return RedirectToAction("HesabAccounts", "Account");
            }
            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> New(HesabAccount model)
        {
            
            if (ModelState.IsValid)
            {
                
                HesabAccount newAcc = new HesabAccount
                {
                    Name = model.Name,
                    Balance = model.Balance,
                    Description = model.Description

                };

                await context.HesabAccounts.AddAsync(newAcc);
                await context.SaveChangesAsync();
                return RedirectToAction("All");
            }
            return View(model);
        }
    }
}
