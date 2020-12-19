using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public float Balance { get; set; }
        public IList<HesabAccountUser> HesabAccountUsers { get; set; }
        public IList<Deposit> Deposits { get; set; }
        public IList<Expense> Expenses { get; set; }
    }
}
