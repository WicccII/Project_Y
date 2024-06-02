using System;
using System.Collections.Generic;

#nullable disable

namespace AccountManageApi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string IdCardPassportNumber { get; set; }
        public string PasswordHash { get; set; }
        public string RoleType { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEmail { get; set; }
        public string OTP { get; set; }
    }
}
