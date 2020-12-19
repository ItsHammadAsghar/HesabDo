using HesabDo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.ViewModels
{
    public class ExpenseViewModel
    {
        public string DataOrder { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int HesabAccountID { get; set; }
        public List<HesabAccount> Items { get; set; }
        public List<Expense> AllExpenses { get; set; }
    }
}
