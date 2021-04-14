using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adutova_Lebedeva_Lab.Models;

namespace Adutova_Lebedeva_Lab.Models
{
    public interface IManager
    {
        public string GetMissionsCompl(List<Mission> missions, Employer employer);
        public IEnumerable<string> GetMissionsComplAll(List<Employer> employers);
        public Mission SetDone(Mission mission);
        public Employer SetMission(Mission mission, Employer employer);
        public Mission SetEmployer(Mission mission, Employer employer);
    }
}
