using Adutova_Lebedeva_Lab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Adutova_Lebedeva_Lab.Controllers
{
    [Route("api/Employer")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly TodoContext _context;

        public EmployerController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Employers
        //[HttpGet]
        //public IEnumerable<Employer> GetEmployers()
        //{
        //    return await _context.
        //    //return Startup.database.GetEmployers();
        //}

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IEnumerable<object> GetEmployers()
        {
            //return await _context.Employers.ToListAsync();
            return _context.Employers.Include(e => e.Missions)
                .Select(e => new { Name = e.Name, Missions = e.Missions.Select(m => m.MissionTask).ToList() }
                ); 
        }


        // GET: api/Employers/5

        //[HttpGet("{id}")]
        //public ActionResult<Employer> GetEmployer(int id)
        //{
        //    var employers = Startup.database.GetEmployers();
        //    var employer = employers.FirstOrDefault(i => i.Id == id);

        //    if (employer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(employer);
        //}
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<Employer>> GetEmployer(long id)
        {
            var Employer = await _context.Employers.FindAsync(id);

            if (Employer == null)
            {
                return NotFound();
            }

            return Employer;
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutEmployer([FromBody]Employer employer)
        {
            _context.Entry(employer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerExists(employer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Employer>> PostEmployer(Employer employer)
        {
            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployer", new { id = employer.Id }, employer);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<Employer>> DeleteEmployer(long id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();

            return employer;
        }

        private bool EmployerExists(long id)
        {
            return _context.Employers.Any(e => e.Id == id);
        }


        [HttpGet("Completed/{EmployerId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<string> GetMissionsCompl(long EmployerId)
        {
            var employer =  await _context.Employers.FindAsync(EmployerId);
            var missions = _context.Missions.Where(m=>m.Employers.Contains(employer)).Where(m=>m.IsComplete==true).Count();
            var missionsall = _context.Missions.Where(m => m.Employers.Contains(employer)).Count();
            //IQueryable<Mission> missions= Startup.database.GetMissions().Where(i => i.IsComplete == comp);
            //var employer = _context.Employers.FindAsync(EmployerId)
            //    .Select(e => new { Name = e.Name, Missions = e.Missions.Select(m => m.MissionTask).ToList() });

            //.Select(e => new { Name = e.Name, Missions = e.Missions.Select(m => m.MissionTask).ToList() }
            //   ); 
            //var employer = _context.Employers.FirstOrDefault(p => p.Id == EmployerId);
            //var EmplMiss = employer.Missions.Where(id => id.IsComplete==true).ToList();
            // var missions = employer.Where(e => e.Id == EmployerId);
            string str = "Выполнено "+ missions + " заданий из " + missionsall;
           
            return str /*_context.Employers.TasksId.Select(id => GetTask(id)?.MissionTask ?? "").ToList()*/;
        }



        [HttpGet("{MissionId}")]
        [Authorize(Roles = "admin, user")]
        public Employer GetEmpl(long MissionId)
        {
            var employ = _context.Employers.FirstOrDefault(p => p.Id == MissionId);
            return employ;
        }

        // PUT: api/Employer/1/AddMission/1
        [HttpPut("{EmployerId}/AddMission/{MissionId}")]
        [Authorize(Roles = "admin, user")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<bool>> AddMissionToEmployer(long EmployerId, long MissionId)
        {
            var employer = await _context.Employers.Include(e=>e.Missions).FirstAsync(e=> e.Id==EmployerId);
            if (employer == null)
            NotFound(new { errorText = $"Employer with id = {EmployerId} was not found." });
            var mission = await _context.Missions.Include(e => e.Employers).FirstAsync(e => e.Id == MissionId);          
            if (mission == null)
            NotFound(new { errorText = $"Mission with id = {MissionId} was not found." });
            /*if (employer.Tasks == null)
            {
            //Mission mission_new = new Mission();
            // mission_new.Id = MissionId;
            //employer.Tasks = taskids;
            }
            else
            employer.Tasks.Add(mission);
            if (mission.Employers == null)
            {
            //List<int> emplids = new List<int>();
            // emplids.Add(EmployerId);
            // mission.Employers = emplids;
            }
            else*/
            employer.Missions.Add(mission);///////////
            mission.Employers.Add(employer);
            _context.Entry(employer).State = EntityState.Modified;
            //_context.Employers.State = EntityState.Modified;
            //context.SaveChanges();
            //_context.Entry(employer).State = EntityState.Modified;
            _context.Entry(mission).State = EntityState.Modified;
            //_context.Employers.Add(employer);
            return await _context.SaveChangesAsync()>0;
            // return await _context.Employers.ToListAsync();
        }

    }
}
