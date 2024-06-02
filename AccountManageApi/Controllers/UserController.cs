using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AccountManageApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountManageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManagementContext _context;

        public UserController(UserManagementContext context)
        {
            _context = context;
        }

        // GET api/users
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Where(u => u.IsDeleted == false && u.IsEmail == true)
                .ToListAsync();
        }

        [HttpGet("Delete")]
        public async Task<ActionResult<IEnumerable<User>>> GetDeleteUsers()
        {
            return await _context.Users
                .Where(u => u.IsDeleted == true)
                .ToListAsync();
        }

        [HttpGet("IsNotEmail")]
        public async Task<ActionResult<IEnumerable<User>>> GetIsNotEmailUsers()
        {
            return await _context.Users
                .Where(u => u.IsEmail == false)
                .ToListAsync();
        }

        // GET api/users/5
        [HttpGet("id", Name = "GetUserById")]
        public ActionResult GetUser(int id)
        {
            var user = this._context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("email", Name = "GetUserByEmail")]
        public ActionResult GetUserByEmail(string email)
        {
            var user = this._context.Users.FirstOrDefault(c => c.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("username", Name = "GetUserByUsername")]
        public ActionResult GetUserByUsername(string username)
        {
            var user = this._context.Users.FirstOrDefault(c => c.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.PasswordHash = EncryptPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT api/users/5
        [HttpPut("Id")]
        public IActionResult UpdateUser(int id, User obj)
        {
            if (id != obj.Id)
            {
                return BadRequest();
            }
            User user1 = _context.Users.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (obj.PasswordHash != user1.PasswordHash)
            {
                obj.PasswordHash = EncryptPassword(obj.PasswordHash);
            }
            User user2 = this._context.Users.AsNoTracking().FirstOrDefault(c => c.Id == id);//loi khi bi entities theo doi
            this._context.Users.Update(obj);
            this._context.SaveChanges();
            return CreatedAtRoute("GetUserById", new { id = obj.Id, obj });
        }

        [HttpPost("SoftDelete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/users/5
        [HttpDelete("id", Name = "HardDelete")]
        public async Task<IActionResult> HardDeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("checklogin")]
        public IActionResult CheckLogin(string username, string password)
        {
            var obj = this._context.Users.AsNoTracking().FirstOrDefault(c => c.Username.Equals(username) && c.PasswordHash.Equals(EncryptPassword(password))
            || c.Email.Equals(username) && c.PasswordHash.Equals(EncryptPassword(password)));
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost("forgotpassword")]
        public IActionResult ForgotPassword(string email)
        {
            var customer = _context.Users.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return NotFound("Không tìm thấy khách hàng với địa chỉ email này.");
            }

            var newPassword = GenerateRandomPassword();
            customer.PasswordHash = EncryptPassword(newPassword); // Encrypt the new password before saving
            _context.SaveChanges();

            SendPasswordEmail(email, newPassword);

            return Ok("Một email chứa mật khẩu mới đã được gửi đến địa chỉ email của bạn.");
        }

        [HttpPost("GetMailSendOTP")]
        public IActionResult GetMailSendOTP(string email)
        {
            var customer = _context.Users.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return NotFound("Không tìm thấy khách hàng với địa chỉ email này.");
            }

            var newOTP = GenerateOTP();
            customer.OTP = newOTP; // Encrypt the new password before saving
            _context.SaveChanges();

            SendOTP(email, customer.OTP);

            return Ok("Một email chứa OTP xác nhận đã được gửi đến địa chỉ email của bạn.");
        }

        [HttpPost("GetMailSendNoff")]
        public IActionResult SendChangeNoff(string email)
        {
            var customer = _context.Users.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return NotFound("Không tìm thấy khách hàng với địa chỉ email này.");
            }
            SendNoff(email);

            return Ok("Một email chứa OTP xác nhận đã được gửi đến địa chỉ email của bạn.");
        }
        private string GenerateRandomPassword()
        {
            // Độ dài của mật khẩu mong muốn
            int length = 10;

            // Tập ký tự cho phép trong mật khẩu
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // Sử dụng StringBuilder để hiệu quả hơn khi làm việc với chuỗi
            StringBuilder sb = new StringBuilder();

            // Sử dụng Random để tạo mật khẩu ngẫu nhiên
            Random random = new Random();

            // Tạo mật khẩu bằng cách chọn ngẫu nhiên các ký tự từ tập hợp validChars
            for (int i = 0; i < length; i++)
            {
                // Chọn một ký tự ngẫu nhiên từ validChars
                int index = random.Next(validChars.Length);

                // Thêm ký tự được chọn vào StringBuilder
                sb.Append(validChars[index]);
            }

            // Trả về mật khẩu đã được tạo ra
            return sb.ToString();
        }

        private string GenerateOTP()
        {
            // Độ dài của mã OTP mong muốn
            int length = 6;

            // Tập số cho phép trong mã OTP
            string validChars = "0123456789";

            // Sử dụng StringBuilder để hiệu quả hơn khi làm việc với chuỗi
            StringBuilder sb = new StringBuilder();

            // Sử dụng Random để tạo mã OTP ngẫu nhiên
            Random random = new Random();

            // Tạo mã OTP bằng cách chọn ngẫu nhiên các số từ tập hợp validChars
            for (int i = 0; i < length; i++)
            {
                // Chọn một số ngẫu nhiên từ validChars
                int index = random.Next(validChars.Length);

                // Thêm số được chọn vào StringBuilder
                sb.Append(validChars[index]);
            }

            // Trả về mã OTP đã được tạo ra
            return sb.ToString();
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
        private void SendPasswordEmail(string email, string newPassword)
        {
            // Gửi email chứa mật khẩu mới
            MailMessage mail = new MailMessage("Ducsieuda@gmail.com", email);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; // hoặc client.UseSsl = true;
            client.Credentials = new System.Net.NetworkCredential("noreplymail.noreply1111@gmail.com", "xcdk ktuh slqg ssqx");
            mail.Subject = "New Password";
            mail.Body = "Your new password is: " + newPassword;
            client.Send(mail);
        }

        private void SendOTP(string email, string otp)
        {
            try
            {
                // Gửi email chứa mật khẩu mới
                MailMessage mail = new MailMessage("Ducsieuda@gmail.com", email);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true; // hoặc client.UseSsl = true;
                client.Credentials = new System.Net.NetworkCredential("noreplymail.noreply1111@gmail.com", "xcdk ktuh slqg ssqx"); // Thay thế YourPassword bằng mật khẩu của bạn
                mail.Subject = "Confirm Email!!!";
                mail.Body = "Your OTP is: " + otp;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // Gửi thông báo lỗi về cho MVC
                throw new Exception("Error sending email: " + ex.Message);
            }
        }

        private void SendNoff(string email)
        {
            try
            {
                // Gửi email chứa mật khẩu mới
                MailMessage mail = new MailMessage("Ducsieuda@gmail.com", email);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true; // hoặc client.UseSsl = true;
                client.Credentials = new System.Net.NetworkCredential("Ducsieuda@gmail.com", "tjgj tslh nbyk xmjk"); // Thay thế YourPassword bằng mật khẩu của bạn
                mail.Subject = "IMPORTANT!!!";
                mail.Body = "HRMs has change your information please check it!!!";
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // Gửi thông báo lỗi về cho MVC
                throw new Exception("Error sending email: " + ex.Message);
            }
        }
    }
}