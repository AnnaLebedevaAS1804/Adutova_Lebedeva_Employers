using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adutova_Lebedeva_Lab.Models
{
    public class Employer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Mission> Missions { get; set; }
    }
}
