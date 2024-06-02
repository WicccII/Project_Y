using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AccountManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace AccountManage.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client = null;
        private string api_User;
        private string api_userid;
        private int count;

        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.api_User = $"https://localhost:5001/api/User";
            this.api_userid = $"https://localhost:5001/api/User/id";
        }

        public async Task<IActionResult> Index()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            int count = TempData["Count"] != null ? (int)TempData["Count"] : 0;
            count = 0;
            // Store the updated count in TempData
            TempData["Count"] = count;
            string id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage response = await client.GetAsync(api_User);
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine("API response is empty");
            }
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code {response.StatusCode}");
            }
            Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}, First Name: {user.FirstName}, Last Name: {user.LastName}");
            return View(user);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            if (!IsEmail())
            {
                TempData["FailMessage"] = "You need to confirm Email to use our facility!";
                return RedirectToAction("index");
            }
            api_User = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_User);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "User Edit Success!";
                return RedirectToAction("index");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int? id, User obj)
        {
            api_User = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_User, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Update Successfull !!!";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public async Task<ActionResult> ChangePassword()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            if (!IsEmail())
            {
                TempData["FailMessage"] = "You need to confirm Email to use our facility!";
                return RedirectToAction("index");
            }
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_User);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "User Edit Success!";
                return RedirectToAction("index");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(User obj)
        {
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_User, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Update Successfull !!!";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public async Task<ActionResult> ReEmail()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            int count = TempData["Count"] != null ? (int)TempData["Count"] : 0;
            count = 0;
            // Store the updated count in TempData
            TempData["Count"] = count;
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_User);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "User Edit Success!";
                return RedirectToAction("index");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> ReEmail(User obj)
        {
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_User, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Update Successfull !!!";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Form()
        {
            return View();
        }
        public async Task<ActionResult> Login(string username, string password)
        {
            Console.WriteLine("this is username:", username);
            api_User = $"https://localhost:5001/api/User/checklogin?username={username}&password={password}";
            var user = new User { Username = username, PasswordHash = password };
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(api_User, content);
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User users = JsonSerializer.Deserialize<User>(data, options);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Role", users.RoleType.ToString());
                HttpContext.Session.SetString("ID", users.Id.ToString());
                HttpContext.Session.SetString("Email", users.Email.ToString());
                HttpContext.Session.SetString("IsEmail", users.IsEmail.ToString());
                HttpContext.Session.SetString("Password", password);
                HttpContext.Session.SetString("Username", username);
                TempData["SuccessMessage"] = "Login Successfull !!!";
                // Chuyển hướng đến trang chủ
                return RedirectToAction("Index");
            }
            else
            {
                TempData["failMessage"] = "Login fail!!! Please check your username and password!!!";
                return View("Form");
            }
        }

        public IActionResult RegisterForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterForm(User obj)
        {
            var email = obj.Email;
            var username = obj.Username;
            api_User = $"https://localhost:5001/api/User/email?email={email}";
            HttpResponseMessage response = await client.GetAsync(api_User);
            if (response.IsSuccessStatusCode)
            {
                TempData["FailMessage"] = "Email is exist !!!";
                return View(obj);
            }
            api_User = $"https://localhost:5001/api/User/username?username={username}";
            response = await client.GetAsync(api_User);
            if (response.IsSuccessStatusCode)
            {
                TempData["FailMessage"] = "Username is exist !!!";
                return View(obj);
            }
            api_User = $"https://localhost:5001/api/User";
            var data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync(api_User, content);
            if (!string.IsNullOrEmpty(data))
            {
                Console.WriteLine("API response is empty");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return View("Form");
            }
            return View(obj);

        }

        public IActionResult FogetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> FogetPassword(string email)
        {
            api_User = $"https://localhost:5001/api/User/email?email={email}";
            HttpResponseMessage response = await client.GetAsync(api_User);
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User users = JsonSerializer.Deserialize<User>(data, options);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Email", users.Email.ToString());
                HttpContext.Session.SetString("IsEmail", users.IsEmail.ToString());
                api_User = $"https://localhost:5001/api/User/forgotpassword?email={email}";
                HttpResponseMessage responsePassword = await client.PostAsync(api_User, null);
                if (responsePassword.IsSuccessStatusCode && IsEmail())
                {
                    TempData["SuccessMessage"] = "We have send new password to your mail,Please check your mail!!!";
                    HttpContext.Session.SetString("Email", email);

                    return RedirectToAction("Form");
                }
                else
                {
                    TempData["FailMessage"] = "Your email is not confirm!!!";
                    return RedirectToAction("Form");
                }
            }
            TempData["FailMessage"] = "Your email is not exist in our system!!!";
            return RedirectToAction("Form");
        }
        public IActionResult CheckSendOTP()
        {
            Console.WriteLine("this is check: " + IsEmail());
            if (IsEmail())
            {
                TempData["SuccessMessage"] = "You have been confirm your email !!!";
                Console.WriteLine("toi da o day");
                return RedirectToAction("index");
            }
            Console.WriteLine("toi da o duoi");
            return RedirectToAction("SendOTP");
        }
        public async Task<IActionResult> SendOTP()
        {
            var email = HttpContext.Session.GetString("Email");
            api_User = $"https://localhost:5001/api/User/GetMailSendOTP?email={email}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(api_User, null);
                int count = TempData["Count"] != null ? (int)TempData["Count"] : 0;
                if (response.IsSuccessStatusCode && count < 3)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("respone: " + responseContent);
                    TempData["SuccessMessage"] = "Check your email! If nothing happens in 60 seconds, please click send again below!!!";

                    // Retrieve the current count from TempData or initialize it to 0
                    count = TempData["Count"] != null ? (int)TempData["Count"] : 0;
                    count++;

                    // Store the updated count in TempData
                    TempData["Count"] = count;
                    Console.WriteLine("respone: " + count);
                    return RedirectToAction("ConfirmEmailForm");
                }
            }
            TempData["FailMessage"] = "We have been sending OTP to your email 3 time but look like your email is not exist, please enter your right email!!!";
            return RedirectToAction("ReEmail");
        }

        public IActionResult ConfirmEmailForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string Otp)
        {
            var email = HttpContext.Session.GetString("Email");
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");
            Console.WriteLine("this is email:" + email);
            api_User = $"https://localhost:5001/api/User/checklogin?username={username}&password={password}";
            var id = HttpContext.Session.GetString("ID");
            var user = new User { Username = username, PasswordHash = password };
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(api_User, content);
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User users = JsonSerializer.Deserialize<User>(data, options);

            if (Otp == users.OTP)
            {
                Console.WriteLine("this is email:" + Otp + "this is in system:" + users.OTP);
                ViewData["Success"] = "Confirm email success!!!";
                return RedirectToAction("Nofftication");
            }
            ViewData["Success"] = "Confirm email fail!!!";
            return RedirectToAction("ConfirmEmailForm");
        }
        public async Task<ActionResult> Nofftication()
        {
            if (!checkLogin())
            {
                return Redirect("Form");
            }
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_User);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Nofftication(User obj)
        {
            var id = HttpContext.Session.GetString("ID");
            api_User = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_User, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Confirm Email Success!";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Logout()
        {
            //xóa hết session đang lưu hiện tại
            HttpContext.Session.Clear();
            return View("form");
        }

        public IActionResult ToHRM()
        {

            return RedirectToAction("Index", "Manager");
        }



        [HttpPost]
        public bool checkLogin()
        {
            var email = HttpContext.Session.GetString("Username");
            var pass = HttpContext.Session.GetString("Password");
            var username = HttpContext.Session.GetString("Password");
            if (email != null && pass != null || username != null && pass != null)
            {
                return true;
            }
            return false;
        }
        [HttpPost]
        public bool IsUser()
        {
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine(role);
            if (role == "User")
            {
                return true;
            }
            return false;
        }

        public bool IsEmail()
        {
            var isEmailString = HttpContext.Session.GetString("IsEmail");
            bool isEmail;
            if (bool.TryParse(isEmailString, out isEmail))
            {
                return isEmail;
            }
            return false;
        }
    }
}