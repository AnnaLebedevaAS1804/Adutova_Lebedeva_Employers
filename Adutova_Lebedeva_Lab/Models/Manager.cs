using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adutova_Lebedeva_Lab.Models;

namespace Adutova_Lebedeva_Lab.Models
{
    public class Manager : IManager
    {
        public string GetMissionsCompl(List<Mission> missions, Employer employer)
        {
            var missionscompl = missions.Where(m => m.Employers.Contains(employer))
                .Where(m => m.IsComplete == true).Count();
            var missionsall = missions.Where(m => m.Employers.Contains(employer)).Count();
            
            string str = "Выполнено " + missionscompl + " заданий из " + missionsall;

            return str;
        }

        public IEnumerable<string> GetMissionsComplAll(List<Employer> employers)
        {
            return employers.Select(e => $"{e.Name}: {e.Missions.Count(m => m.IsComplete)}/{e.Missions.Count}");
        }

        public Mission SetDone(Mission mission)
        {
            mission.IsComplete = true;
            return mission;
        }

        public Mission SetEmployer(Mission mission, Employer employer)
        {
            if (mission.Employers == null)
                mission.Employers = new List<Employer>();
           mission.Employers.Add(employer);
            return mission;
        }

        public Employer SetMission(Mission mission, Employer employer)
        {
            if (employer.Missions == null)
                employer.Missions= new List<Mission>();
            employer.Missions.Add(mission);
            return employer;
        }

    }
}
