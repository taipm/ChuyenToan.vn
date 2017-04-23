using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Giới thiệu")]
        public string About { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}