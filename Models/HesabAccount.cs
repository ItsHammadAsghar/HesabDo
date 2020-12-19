using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.Models
{
    public class HesabAccount
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Balance { get; set; }
        public string Description { get; set; }

        public IList<HesabAccountUser> HesabAccountUsers { get; set; }

        public IList<Deposit> Deposits { get; set; }
        public IList<Expense> Expenses { get; set; }

    }
}
