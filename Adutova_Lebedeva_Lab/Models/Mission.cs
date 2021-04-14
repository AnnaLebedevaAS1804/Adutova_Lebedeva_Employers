using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adutova_Lebedeva_Lab.Models
{
    public class Mission
    {
        public long Id { get; set; }
        public string MissionTask { get; set; } //содержание
        public bool IsComplete { get; set; } = false;
        public List<Employer> Employers { get; set; }

    }
}
