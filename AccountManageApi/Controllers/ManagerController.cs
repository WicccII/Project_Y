using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AccountManageApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountManageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly UserManagementContext _context;

        public ManagerController(UserManagementContext context)
        {
            _context = context;
        }

        // GET api/Managers
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManagers()
        {
            return await _context.Managers
                .Where(u => u.IsDeleted == false && u.RoleType == "HRM")
                .ToListAsync();
        }

        [HttpGet("Delete")]
        public async Task<ActionResult<IEnumerable<Manager>>> GetDeleteManagers()
        {
            return await _context.Managers
                .Where(u => u.IsDeleted == true)
                .ToListAsync();
        }

        // GET api/Managers/5
        [HttpGet("id", Name = "GetManagerById")]
        public ActionResult GetManager(int id)
        {
            var Manager = this._context.Managers.Find(id);
            if (Manager == null)
            {
                return NotFound();
            }
            return Ok(Manager);
        }

        [HttpGet("email", Name = "GetManagerByEmail")]
        public ActionResult GetManagerByEmail(string email)
        {
            var Manager = this._context.Managers.FirstOrDefault(c => c.Email == email);
            if (Manager == null)
            {
                return NotFound();
            }
            return Ok(Manager);
        }

        [HttpGet("username", Name = "GetManagerByUsername")]
        public ActionResult GetManagerByUsername(string username)
        {
            var Manager = this._context.Managers.FirstOrDefault(c => c.Username == username);
            if (Manager == null)
            {
                return NotFound();
            }
            return Ok(Manager);
        }

        // POST api/Managers
        [HttpPost]
        public async Task<ActionResult<Manager>> CreateManager(Manager Manager)
        {
            Manager.PasswordHash = EncryptPassword(Manager.PasswordHash);
            _context.Managers.Add(Manager);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetManager), new { id = Manager.Id }, Manager);
        }

        // PUT api/Managers/5
        [HttpPut("Id")]
        public IActionResult UpdateManager(int id, Manager obj)
        {
            if (id != obj.Id)
            {
                return BadRequest();
            }
            Manager manager = _context.Managers.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (obj.PasswordHash != manager.PasswordHash)
            {
                obj.PasswordHash = EncryptPassword(obj.PasswordHash);
            }
            Manager Manager1 = this._context.Managers.AsNoTracking().FirstOrDefault(c => c.Id == id);//loi khi bi entities theo doi
            this._context.Managers.Update(obj);
            this._context.SaveChanges();
            return CreatedAtRoute("GetManagerById", new { id = obj.Id, obj });
        }

        [HttpPost("SoftDelete")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var Manager = await _context.Managers.FindAsync(id);
            if (Manager == null)
            {
                return NotFound();
            }
            Manager.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/Managers/5
        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDeleteManager(int id)
        {
            var Manager = await _context.Managers.FindAsync(id);
            if (Manager == null)
            {
                return NotFound();
            }
            _context.Managers.Remove(Manager);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("ManagerChecklogin")]
        public IActionResult ManagerCheckLogin(string username, string password)
        {
            var obj = this._context.Managers.AsNoTracking().FirstOrDefault(c => c.Username.Equals(username) && c.PasswordHash.Equals(EncryptPassword(password)) || c.Email.Equals(username) && c.PasswordHash.Equals(EncryptPassword(password)));
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }
        private string EncryptPassword(string password)
        {
            // Encrypt the password logic (You can use any encryption algorithm like MD5, SHA256, etc.)
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}