using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COF.API.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "UserId")]
        public string UserId { get; set; }


        [Display(Name = "Username")]
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu có độ dài tối thiểu là 6.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không trùng.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vị trí nhân viên là bắt buộc.")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

       // [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

       // [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
       
        [Display(Name = "Shop")]
        public int? ShopId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class UpdateAccountViewModel
    {
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Vị trí nhân viên là bắt buộc.")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

        // [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        // [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Shop")]
        public int? ShopId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
