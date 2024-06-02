using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

#nullable disable

namespace AccountManage.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [MaxLength(5, ErrorMessage = "Zip code cannot exceed 5 characters")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(0|84)[0-9]{9,10}$", ErrorMessage = "Phone number is not in the correct format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email address")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "ID card or passport number is required")]
        [MaxLength(50, ErrorMessage = "ID card or passport number cannot exceed 50 characters")]
        public string IdCardPassportNumber { get; set; }

        [Required(ErrorMessage = "Password hash is required")]
        [MaxLength(100, ErrorMessage = "Password hash cannot exceed 100 characters")]
        public string PasswordHash { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "ConfirmPassword is required and not null.")]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "Password not match!")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public string RoleType { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEmail { get; set; }
        [JsonPropertyName("Otp")]
        public string OTP { get; set; }

    }


}
