using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project_cuoiky.Models;

namespace project_cuoiky.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter your first name.")]
            [Display(Name = "First Name")]
            public string FristName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your last name.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your email.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Phone(ErrorMessage = "Invalid phone number.")]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter a password.")]
            [StringLength(
                100,
                MinimumLength = 6,
                ErrorMessage = "Password must be at least 6 characters."
            )]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please confirm your password.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare(
                "Password",
                ErrorMessage = "Password and confirmation password do not match."
            )]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new AppUser
            {
                UserName = Input.Email.Trim(),
                Email = Input.Email.Trim(),

                FristName = Input.FristName.Trim(),
                LastName = Input.LastName.Trim(),

                PhoneNumber = Input.PhoneNumber?.Trim(),
                Address = Input.Address?.Trim() ?? string.Empty,

                // Chưa upload avatar thì để rỗng
                Image = string.Empty
            };

            var result = await _userManager.CreateAsync(
                user,
                Input.Password
            );

            if (result.Succeeded)
            {
                // Đăng ký thành công -> đăng nhập luôn
                await _signInManager.SignInAsync(
                    user,
                    isPersistent: false
                );

                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description
                );
            }

            return Page();
        }
    }
}