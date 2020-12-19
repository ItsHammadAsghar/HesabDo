using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.Models
{
    public class Deposit
    {
        public int ID { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public HesabAccount HesabAccount { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
