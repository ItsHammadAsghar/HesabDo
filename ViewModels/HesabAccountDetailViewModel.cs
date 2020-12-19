using HesabDo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.ViewModels
{
    public class HesabAccountDetailViewModel
    {
        
        public HesabAccount HesabAccount { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<Deposit> Deposits { get; set; }
    }
}