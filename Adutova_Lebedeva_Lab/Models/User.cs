using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adutova_Lebedeva_Lab.Models
{
    public class User
    {
        public int id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
