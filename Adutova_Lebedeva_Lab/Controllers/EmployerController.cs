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
        private readonly IManager _manager;

        public EmployerController(TodoContext context, IManager manager)
        {
            _context = context;
            _manager = manager;
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IEnumerable<object> GetEmployers()
        {
            //return await _context.Employers.ToListAsync();
            return _context.Employers.Include(e => e.Missions)
                .Select(e => new { Name = e.Name, Missions = e.Missions.Select(m => m.MissionTask).ToList() }
                );
        }

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
        public async Task<IActionResult> PutEmployer([FromBody] Employer employer)
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
        [Authorize(Roles = "admin")]
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
            var employer = await _context.Employers.FindAsync(EmployerId);
            return _manager.GetMissionsCompl(_context.Missions.ToList(), employer);
        }

        [HttpGet("Completed")]
        [Authorize(Roles = "admin, user")]
        public IEnumerable<string> GetMissionsCompl()
        {
            return _manager.GetMissionsComplAll(_context.Employers.ToList());
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
            var mission = await _context.Missions.FindAsync(MissionId);
            if (mission == null)
                NotFound(new { errorText = $"Mission with id = {MissionId} was not found." });
            var employer = _manager.SetMission(mission, await _context.Employers.FindAsync(EmployerId));
            mission = _manager.SetEmployer(mission, await _context.Employers.FindAsync(EmployerId));
            _context.Entry(employer).State = EntityState.Modified;
            _context.Entry(mission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerExists(EmployerId))
                {
                    return NotFound(new { errorText = $"Employerwith id = {EmployerId} was not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }




        //    var employer = await _context.Employers.Include(e=>e.Missions).FirstAsync(e=> e.Id==EmployerId);
        //        if (employer == null)
        //        NotFound(new { errorText = $"Employer with id = {EmployerId} was not found." });
        //        var mission = await _context.Missions.Include(e => e.Employers).FirstAsync(e => e.Id == MissionId);          
        //        if (mission == null)
        //        NotFound(new { errorText = $"Mission with id = {MissionId} was not found." });

        //        employer.Missions.Add(mission);///////////
        //        mission.Employers.Add(employer);
        //        _context.Entry(employer).State = EntityState.Modified;
        //        //_context.Employers.State = EntityState.Modified;
        //        //context.SaveChanges();
        //        //_context.Entry(employer).State = EntityState.Modified;
        //        _context.Entry(mission).State = EntityState.Modified;
        //        //_context.Employers.Add(employer);
        //        return await _context.SaveChangesAsync()>0;
        //        // return await _context.Employers.ToListAsync();
        //    }

        //}
    }
}
