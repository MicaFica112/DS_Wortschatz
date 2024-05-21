using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Wortschatz.Models
{
    public class User
    {   
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int IsAdmin { get; set; }
        public string? Email { get; set; }
    }
}
