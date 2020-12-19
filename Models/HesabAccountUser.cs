using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HesabDo.Models
{
    public class HesabAccountUser
    {
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int HesabAccountID { get; set; }
        public HesabAccount HesabAccount { get; set; }

    }
}
