using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WinProApp.Areas.Identity.Data;

namespace WinProApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles ="Administrator")]
    public class ChangeUserPasswordModel : PageModel
    {
        private readonly UserManager<WinProAppUser> _userManager;
      //private readonly SignInManager<WinProAppUser> _signInManager;
        private readonly ILogger<ChangeUserPasswordModel> _logger;

        public ChangeUserPasswordModel(
            UserManager<WinProAppUser> userManager,
            ILogger<ChangeUserPasswordModel> logger)
        {
            _userManager = userManager;
//
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Id { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public async Task OnGetAsync(string id)
        {
            var userInfo = await _userManager.FindByIdAsync(id);
            ViewData["Id"] = id;
            ViewData["UserFirstName"] = userInfo.FirstName;
            ViewData["UserLastName"] = userInfo.LastName;
            ViewData["UserName"] = userInfo.UserName;
            ViewData["Email"] = userInfo.Email;

           // return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            
                var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);
           // var result =  await _userManager.AddPasswordAsync(user, Input.NewPassword);
               // await _userManager.UpdateAsync(user);
            return Redirect("/Administrator/ManageAccounts");
        }


        }
}
