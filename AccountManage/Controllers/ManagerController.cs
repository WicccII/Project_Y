using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AccountManage.Models;
using System.Text;
using System.Net;

namespace AccountManage.Controllers
{
    [Route("Manager/[action]")]
    public class ManagerController : Controller
    {
        private readonly HttpClient client = null;
        private string api_Manager;
        private string api_Managerid;

        public ManagerController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.api_Manager = $"https://localhost:5001/api/Manager";
            this.api_Managerid = $"https://localhost:5001/api/Manager/id";
        }
        public async Task<IActionResult> IndexAsync()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/Active";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<User> list = JsonSerializer.Deserialize<List<User>>(data, options);
                return View(list);
            }
        }

        public async Task<IActionResult> IndexUserNotEmail()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/IsNotEmail";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<User> list = JsonSerializer.Deserialize<List<User>>(data, options);
                return View(list);
            }
        }

        public async Task<IActionResult> IndexFS()
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/Active";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<User> list = JsonSerializer.Deserialize<List<User>>(data, options);
                return View(list);
            }
        }

        public async Task<IActionResult> Staff()
        {
            if (!checkLogin() && !IsAdmin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/Manager/Active";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<Manager> list = JsonSerializer.Deserialize<List<Manager>>(data, options);
                return View(list);
            }
        }

        public async Task<IActionResult> Recycle()
        {
            if (!checkLogin() && !IsAdmin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/Delete";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<User> list = JsonSerializer.Deserialize<List<User>>(data, options);
                return View(list);
            }
        }

        public async Task<IActionResult> StaffRecycle()
        {
            if (!checkLogin() && !IsAdmin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/manager/Delete";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return View();
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<Manager> list = JsonSerializer.Deserialize<List<Manager>>(data, options);
                return View(list);
            }
        }


        public async Task<ActionResult> AdminEdit(int? id)
        {
            if (!checkLogin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> AdminEdit(int? id, User obj)
        {
            api_Manager = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                api_Manager = $"https://localhost:5001/api/User/id?id={id}";
                respone = await client.GetAsync(api_Manager);
                data = await respone.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                User user = JsonSerializer.Deserialize<User>(data, options);
                api_Manager = $"https://localhost:5001/api/User/GetMailSendNoff?email={user.Email}";
                HttpResponseMessage responsePassword = await client.PostAsync(api_Manager, null);
                if (responsePassword.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "User Edit Success!";
                    return RedirectToAction("index");
                }
            }
            return View(obj);
        }

        public async Task<ActionResult> StaffEdit(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/Manager/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Manager manager = JsonSerializer.Deserialize<Manager>(data, options);
            return View(manager);
        }

        [HttpPost]
        public async Task<ActionResult> StaffEdit(int? id, Manager obj)
        {
            api_Manager = $"https://localhost:5001/api/Manager/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "User Edit Success!";
                return RedirectToAction("staff");

            }
            return View(obj);
        }

        public async Task<ActionResult> StaffEditUser(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> StaffEditUser(int? id, User obj)
        {
            api_Manager = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                api_Manager = $"https://localhost:5001/api/User/id?id={id}";
                respone = await client.GetAsync(api_Manager);
                data = await respone.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                User user = JsonSerializer.Deserialize<User>(data, options);
                api_Manager = $"https://localhost:5001/api/User/GetMailSendNoff?email={user.Email}";
                HttpResponseMessage responsePassword = await client.PostAsync(api_Manager, null);
                if (responsePassword.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "User Edit Success!";
                    return RedirectToAction("index");
                }
            }
            return View(obj);
        }
        public async Task<ActionResult> SoftDelete(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> SoftDelete(int? id, User obj)
        {
            api_Manager = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Delete Success!";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public async Task<ActionResult> StaffSoftDelete(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/Manager/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Manager manager = JsonSerializer.Deserialize<Manager>(data, options);
            return View(manager);
        }

        [HttpPost]
        public async Task<ActionResult> StaffSoftDelete(int? id, Manager obj)
        {
            api_Manager = $"https://localhost:5001/api/Manager/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Delete Success!";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public async Task<ActionResult> Recover(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Recover(int? id, User obj)
        {
            api_Manager = $"https://localhost:5001/api/User/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Recover Success!";
                return RedirectToAction("index");
            }
            return View(obj);
        }


        public async Task<ActionResult> StaffRecover(int? id)
        {
            if (!checkLogin() && !IsAdmin())
            {
                return Redirect("Form");
            }
            api_Manager = $"https://localhost:5001/api/Manager/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Manager manager = JsonSerializer.Deserialize<Manager>(data, options);
            return View(manager);
        }

        [HttpPost]
        public async Task<ActionResult> StaffRecover(int? id, Manager obj)
        {
            api_Manager = $"https://localhost:5001/api/Manager/Id?id={id}";
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await client.PutAsync(api_Manager, content);
            if (respone.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TempData["SuccessMessage"] = "Recover Success!";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Form()
        {
            return View();
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            User user = JsonSerializer.Deserialize<User>(data, options);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            api_Manager = $"https://localhost:5001/api/User/id?id={id}";
            try
            {

                HttpResponseMessage response = await client.DeleteAsync(api_Manager);


                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "user Delete Success!";

                    return RedirectToAction("Index");
                }
                else
                {
                    // Xử lý kết quả nếu xóa không thành công, ví dụ hiển thị thông báo lỗi
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                System.Console.WriteLine(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> DeleteStaff(int? id)
        {
            if (!checkLogin())
            {
                return RedirectToAction("Form");
            }
            api_Manager = $"https://localhost:5001/api/Manager/id?id={id}";
            HttpResponseMessage respone = await client.GetAsync(api_Manager);
            string data = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Manager manager = JsonSerializer.Deserialize<Manager>(data, options);
            return View(manager);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStaff(int id)
        {
            api_Manager = $"https://localhost:5001/api/Manager/HardDelete?id={id}";
            try
            {
                // Tạo yêu cầu DELETE
                HttpResponseMessage response = await client.DeleteAsync(api_Manager);

                // Kiểm tra kết quả trả về từ endpoint API
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "user Delete Success!";
                    // Xử lý kết quả nếu xóa thành công, ví dụ chuyển hướng đến trang danh sách
                    return RedirectToAction("Index");
                }
                else
                {
                    // Xử lý kết quả nếu xóa không thành công, ví dụ hiển thị thông báo lỗi
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                System.Console.WriteLine(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> Login(string username, string password)
        {

            api_Manager = $"https://localhost:5001/api/Manager/ManagerChecklogin?username={username}&password={password}";
            var manager = new Manager { Username = username, PasswordHash = password };
            var content = new StringContent(JsonSerializer.Serialize(manager), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(api_Manager, content);
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Manager Managers = JsonSerializer.Deserialize<Manager>(data, options);
            Console.WriteLine($"this is username: {Managers.Username}");
            if (response.IsSuccessStatusCode)
            {

                HttpContext.Session.SetString("ManagerID", Managers.Id.ToString());
                HttpContext.Session.SetString("Role", Managers.RoleType.ToString());
                HttpContext.Session.SetString("Password", password);
                HttpContext.Session.SetString("Username", username);
                TempData["SuccessMessage"] = "Login Successfull !!!";
                if (Managers.RoleType == "Admin")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexFS");
                }
                // Chuyển hướng đến trang chủ

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
        public async Task<IActionResult> RegisterForm(Manager obj)
        {
            var email = obj.Email;
            var username = obj.Username;
            api_Manager = $"https://localhost:5001/api/Manager/email?email={email}";
            HttpResponseMessage response = await client.GetAsync(api_Manager);
            if (response.IsSuccessStatusCode)
            {
                TempData["FailMessage"] = "Email is exist !!!";
                return View(obj);
            }
            api_Manager = $"https://localhost:5001/api/manager/username?username={username}";
            response = await client.GetAsync(api_Manager);
            if (response.IsSuccessStatusCode)
            {
                TempData["FailMessage"] = "Username is exist !!!";
                return View(obj);
            }
            api_Manager = $"https://localhost:5001/api/Manager";
            var data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync(api_Manager, content);
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
        public IActionResult ToUser()
        {

            return RedirectToAction("Index", "User");
        }

        public IActionResult Logout()
        {
            //xóa hết session đang lưu hiện tại
            HttpContext.Session.Clear();
            return View("form");
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
        public bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine(role);
            if (role == "Admin")
            {
                return true;
            }
            return false;
        }
    }
}