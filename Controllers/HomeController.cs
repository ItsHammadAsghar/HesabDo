using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HesabDo.Models;
using Microsoft.AspNetCore.Authorization;
using HesabDo.Data;
using Microsoft.AspNetCore.Identity;
using HesabDo.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HesabDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
                                        AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeleteDeposit(string id)
        {
            var dep = await context.Deposits.Where(e => e.ID.ToString() == id).Include(e => e.HesabAccount).FirstAsync();
            var hb = await context.HesabAccounts.FindAsync(dep.HesabAccount.ID);
            hb.Balance -= dep.Amount;
            context.Deposits.Remove(dep);
            
            await context.SaveChangesAsync();

            return RedirectToAction("Deposits");
        }

        public async Task<IActionResult> DeleteExpense(string id)
        {
            var exp = await context.Expenses.Where(e => e.ID.ToString() == id).Include(e => e.HesabAccount).FirstAsync();
            var hb = await context.HesabAccounts.FindAsync(exp.HesabAccount.ID);
            hb.Balance += exp.Amount;
            context.Expenses.Remove(exp);
            await context.SaveChangesAsync();

            return RedirectToAction("Expense");
        }


        public async Task<IActionResult> Sort(string sortType, string table)
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime dateTomorrow = dateTimeNow.Date.AddDays(1);
            DateTime dateWeek = dateTimeNow.Date.AddDays(7);
            var deposits = from d in context.Deposits
                           select d;
            var expenses = from e in context.Expenses
                           select e;
            var items = await context.HesabAccounts.ToListAsync();
            switch(table)
            {
                
                case "Deposit":
                    switch(sortType)
                    {
                        case "DateTime":
                            deposits = deposits.OrderBy(e => e.DateTime);
                            DepositViewModel model = new DepositViewModel
                            {
                                Deposits = await deposits.ToListAsync(),
                                Items = items,
                                DataOrder = "DateTime"
                            };
                            return View("Deposit", model);
                        case "DateTimeDescending":
                            deposits = deposits.OrderByDescending(e => e.DateTime);
                            DepositViewModel model2 = new DepositViewModel
                            {
                                Deposits = await deposits.ToListAsync(),
                                Items = items,
                                DataOrder = "DateTime"
                            };
                            return View("Deposit", model2);
                        case "Today":
                            deposits = deposits.Where(e => e.DateTime < dateTomorrow).OrderByDescending(e => e.DateTime);
                            DepositViewModel model3 = new DepositViewModel
                            {
                                Deposits = await deposits.ToListAsync(),
                                Items = items,
                                DataOrder = "Today"
                            };
                            return View("Deposit", model3);
                        case "All":
                            deposits = deposits.OrderByDescending(e => e.DateTime);
                            DepositViewModel model4 = new DepositViewModel
                            {
                                Deposits = await deposits.ToListAsync(),
                                Items = items,
                                DataOrder = "All"
                            };
                            return View("Deposit", model4);
                        case "Week":
                            deposits = deposits.Where(e => e.DateTime < dateWeek).OrderByDescending(e => e.DateTime);
                            DepositViewModel model5 = new DepositViewModel
                            {
                                Deposits = await deposits.ToListAsync(),
                                Items = items,
                                DataOrder = "Week"
                            };
                            return View("Deposit", model5);
                    }
                    break;
                case "Expense":
                    switch(sortType)
                    {
                        case "DateTime":
                            expenses = expenses.OrderBy(e => e.DateTime);
                            ExpenseViewModel model = new ExpenseViewModel
                            {
                                AllExpenses = await expenses.ToListAsync(),
                                Items = items,
                                DataOrder = "DateTime"
                            };
                            return View("Expense", model);

                        case "DateTimeDescending":
                            expenses = expenses.OrderByDescending(e => e.DateTime);
                            ExpenseViewModel model2 = new ExpenseViewModel
                            {
                                AllExpenses = await expenses.ToListAsync(),
                                Items = items,
                                DataOrder = "DateTime"
                            };
                            return View("Expense", model2);
                        case "Today":
                            expenses = expenses.Where(e => e.DateTime < dateTomorrow).OrderByDescending(e => e.DateTime);
                            ExpenseViewModel model3 = new ExpenseViewModel
                            {
                                AllExpenses = await expenses.ToListAsync(),
                                Items = items,
                                DataOrder = "Today"
                            };
                            return View("Expense", model3);
                        case "All":
                            expenses = expenses.OrderByDescending(e => e.DateTime);
                            ExpenseViewModel model4 = new ExpenseViewModel
                            {
                                AllExpenses = await expenses.ToListAsync(),
                                Items = items,
                                DataOrder = "All"
                            };
                            return View("Expense", model4);
                        case "Week":
                            expenses = expenses.Where(e => e.DateTime < dateWeek).OrderByDescending(e => e.DateTime);
                            ExpenseViewModel model5 = new ExpenseViewModel
                            {
                                AllExpenses = await expenses.ToListAsync(),
                                Items = items,
                                DataOrder = "Week"
                            };
                            return View("Expense", model5);
                        
                            
                    }
                    break;
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Deposit()
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime dateTomorrow = dateTimeNow.Date.AddDays(1);
            var items = await context.HesabAccounts.ToListAsync();
            var user = await userManager.GetUserAsync(HttpContext.User);
            var data = await context.Deposits.Where(e => e.ApplicationUser.Id == user.Id).Where(e => e.DateTime < dateTomorrow)
                                                .Include(e => e.HesabAccount)
                                                .OrderByDescending(e => e.DateTime).ToListAsync();
            DepositViewModel model = new DepositViewModel
            {
                Deposits = data,
                DateTime = DateTime.UtcNow,
                DataOrder = "DateTimeDescending",
                Items = items
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                var ha = await context.HesabAccounts.Where(e => e.ID == model.HesabAccountID).FirstAsync();

                Deposit newAcc = new Deposit
                {
                    Amount = model.Amount,
                    Description = model.Description,
                    DateTime = model.DateTime,
                    HesabAccount = ha,
                    ApplicationUser = user


                };
                ha.Balance += model.Amount; 

                await context.Deposits.AddAsync(newAcc);
                await context.SaveChangesAsync();
                
                return RedirectToAction("Deposit");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Expense()
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime dateTomorrow = dateTimeNow.Date.AddDays(1);
            var items = await context.HesabAccounts.ToListAsync();
            var user = await userManager.GetUserAsync(HttpContext.User);
            var data = await context.Expenses.Where(e => e.ApplicationUser.Id == user.Id).Where(e => e.DateTime < dateTomorrow)
                                                .Include(e => e.HesabAccount).OrderByDescending(e => e.DateTime).ToListAsync();
            ExpenseViewModel model = new ExpenseViewModel
            {
                AllExpenses = data,
                DateTime = DateTime.UtcNow,
                Items = items,
                DataOrder = "DateTimeDescending"
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Expense(ExpenseViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                var ha = await context.HesabAccounts.Where(e => e.ID == model.HesabAccountID).FirstAsync();

                Expense newAcc = new Expense
                {
                    Amount = model.Amount,
                    Description = model.Description,
                    DateTime = model.DateTime,
                    HesabAccount = ha,
                    ApplicationUser = user


                };
                ha.Balance -= model.Amount;

                await context.Expenses.AddAsync(newAcc);
                await context.SaveChangesAsync();
                
                return RedirectToAction("Expense");
            }
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
