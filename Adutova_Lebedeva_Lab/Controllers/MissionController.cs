using Adutova_Lebedeva_Lab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Adutova_Lebedeva_Lab.Controllers
{
    [Route("api/Mission")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly TodoContext _context;

        public MissionController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Mission
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public  IEnumerable<object> GetMission()
        {
           
            return _context.Missions.Include(e => e.Employers)
                .Select(e => new { MissionTask = e.MissionTask, Employers = e.Employers.Select(m => m.Name).ToList() }
                );
            //return await _context.Missions.ToListAsync();
        }

        // GET: api/Mission/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<Mission>> GetMission(long id)
        {
            var Mission = await _context.Missions.FirstOrDefaultAsync(i => i.Id == id);
            //            var Mission = _context.Mission.FirstOrDefault(i => i.Id == id);

            if (Mission == null)
            {
                return NotFound();
            }

            return Mission;
        }

        // PUT: api/Mission/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutMission([FromBody]Mission mission)
        {
            

            _context.Entry(mission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionExists(mission.Id))
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

        // POST: api/Mission
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Mission>> PostMission(Models.Mission Mission)
        {
            _context.Missions.Add(Mission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMission", new { id = Mission.Id }, Mission);
        }

        // DELETE: api/Mission/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Mission>> DeleteMission(long id)
        {
            var Mission = await _context.Missions.FindAsync(id);
            if (Mission == null)
            {
                return NotFound();
            }

            _context.Missions.Remove(Mission);
            await _context.SaveChangesAsync();

            return Mission;
        }

        private bool MissionExists(long id)
        {
            return _context.Missions.Any(e => e.Id == id);
        }
        /// <summary>
        // GET: api/Mission
        [HttpGet("Completed/{comp}")]
        [Authorize(Roles = "admin, user")]
        public IEnumerable<object> GetMissionCompl(bool comp)
        {
            //IQueryable<Mission> missions= Startup.database.GetMissions().Where(i => i.IsComplete == comp);
            var missions = _context.Missions.Where(i => i.IsComplete == comp).Include(e => e.Employers)
                .Select(e => new { MissionTask = e.MissionTask, Employers = e.Employers.Select(m => m.Name).ToList() }
                );
            //var missions = await _context.Missions.ToListAsync();
            //var missionCompl = missions.Where(i => i.IsComplete == comp).ToList();
            //var mission = missions.Where(i => i.IsComplete == comp);
            return missions;
        }



        [HttpGet("{EmployerId}")]
        [Authorize(Roles = "admin, user")]
        public Mission GetTask(long EmployerId)
        {
            var task = _context.Missions.FirstOrDefault(p => p.Id == EmployerId);
            return task;
        }
    }
}

