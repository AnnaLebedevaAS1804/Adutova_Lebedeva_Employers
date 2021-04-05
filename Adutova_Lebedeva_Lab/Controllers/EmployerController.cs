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
        public async Task<ActionResult<Employer>> GetEmployer(long id)
        {
            var Employer = await _context.Employers.FindAsync(id);

            if (Employer == null)
            {
                return NotFound();
            }

            return Employer;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployer(long id, Employer employer)
        {
            if (id != employer.Id)
            {
                return BadRequest();
            }

            _context.Entry(employer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employers
        //[HttpPost]
        //public async Task<ActionResult<Employer>> PostEmployer(Employer employer, List<long> id)
        //{

        //    List<Mission> selected_missions = new List<Mission>();
        //    foreach (var miss in id)
        //    {
        //        var mission = await _context.Missions.FindAsync(miss);
        //        if (id != null)
        //            selected_missions.Add(mission);
        //    }
        //    employer.Missions = selected_missions;
        //    _context.Employers.Add(employer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployer", new { id = employer.Id }, employer);
        //}

        //// POST: api/Employer/1/AddMission/1
        //[HttpPost("{EmployerId}/AddMission/{MissionId}")]
        //public async Task<ActionResult<Employer>> AddMissionToEmployer(long EmployerId, long MissionId)
        //{

        //    Employer employer = await _context.Employers.FindAsync(EmployerId);

        //    if (employer == null)
        //        NotFound(new { errorText = $"Employer with id = {EmployerId} was not found." });

        //    Mission mission = await _context.Missions.FindAsync(MissionId);
        //    if (mission == null)
        //        NotFound(new { errorText = $"Mission with id = {MissionId} was not found." });

        //    var employ = new Employer();
        //    employ = await _context.Missions.FindAsync(mission); ////System.NullReferenceException: 'Object reference not set to an instance of an object.'

        //    //_context.Employers.Add(employer);
        //    //await _context.SaveChangesAsync();

        //    Employer empl = new Employer();
        //    empl = await _context.Employers.FindAsync(employer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployer", new { id = employer.Id }, employer);
        //}

        // DELETE: api/Employers/5
        // POST: api/Employers
        
        [HttpPost]
        public async Task<ActionResult<Employer>> PostEmployer(Employer employer)
        {
            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployer", new { id = employer.Id }, employer);
        }

        [HttpDelete("{id}")]
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
        //public Mission GetTask(long EmployerId)
        //{
        //    var task = _context.Missions.FirstOrDefault(p => p.Id == EmployerId);
        //    return task;
        //}
        //public List<string> GetEmployersMissions(long EmployerId)
        //{
        //    var Employer = _context.Employers.FirstOrDefault(p => p.Id == EmployerId);
        //    var EmplMiss = Employer.Missions.Select(id => GetTask(id)?.MissionTask ?? "").ToList();
        //    if (EmplMiss == null)
        //    {
        //        return null;
        //    }
        //    return EmplMiss /*_context.Employers.TasksId.Select(id => GetTask(id)?.MissionTask ?? "").ToList()*/;
        //}

        [HttpGet("Completed/{EmployerId}")]
        public int GetMissionsCompl(long EmployerId)
        {
            //IQueryable<Mission> missions= Startup.database.GetMissions().Where(i => i.IsComplete == comp);
            var Employer = _context.Employers.FirstOrDefault(p => p.Id == EmployerId);
            var EmplMiss = Employer.Missions.Where(id => id.IsComplete==true).ToList();
            int nummissions = EmplMiss.Count();
            if (EmplMiss == null)
            {
                return 0;
            }
            return nummissions /*_context.Employers.TasksId.Select(id => GetTask(id)?.MissionTask ?? "").ToList()*/;
        }



        [HttpGet("{MissionId}")]
        public Employer GetEmpl(long MissionId)
        {
            var employ = _context.Employers.FirstOrDefault(p => p.Id == MissionId);
            return employ;
        }

        // PUT: api/Employer/1/AddMission/1
        [HttpPut("{EmployerId}/AddMission/{MissionId}")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<bool>> AddMissionToEmployer(long EmployerId, long MissionId)
        {
            var employer = await _context.Employers.Include(e=>e.Missions).FirstAsync(e=> e.Id==EmployerId);
            if (employer == null)
            NotFound(new { errorText = $"Employer with id = {EmployerId} was not found." });
            var mission = await _context.Missions.Include(e => e.Employers).FirstAsync(e => e.Id == MissionId);          if (mission == null)
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
