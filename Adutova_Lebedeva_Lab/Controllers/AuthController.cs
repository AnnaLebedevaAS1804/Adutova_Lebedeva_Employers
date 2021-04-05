using Adutova_Lebedeva_Lab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Adutova_Lebedeva_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoContext _context;
        public AuthController(TodoContext context)
        {
            _context = context;
        }
        public static List<User> Users { get; } = new List<User>
        {
            new User(){ id =1, Login = "user", Password = "user" },
            new User(){ id=2, Login = "admin", Password = "admin" },
        };


        // GET: api/Users
        [HttpGet("api/Users")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("api/Users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound(new { errorText = $"User with id = {id} was not found." });
            }

            return User;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("api/Users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUser(int id, User User)
        {
            if (id != User.id)
            {
                return BadRequest(new { errorText = "ID and User.ID are different." });
            }

            _context.Entry(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(new { errorText = $"User with id = {id} was not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("api/Users")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> PostUser(User User)
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = User.id }, User);
        }

        // DELETE: api/Users/5
        [HttpDelete("api/Users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User == null)
            {
                return NotFound(new { errorText = $"User with id = {id} was not found." });
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
   

    }
}

