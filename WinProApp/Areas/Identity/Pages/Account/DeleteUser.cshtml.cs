using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WinProApp.Areas.Identity.Data;

namespace WinProApp.Areas.Identity.Pages.Account
{
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<WinProAppUser> _userManager;
        private readonly ILogger<EditUserModel> _logger;

        public DeleteUserModel(
            UserManager<WinProAppUser> userManager,
            ILogger<EditUserModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        public class InputModel
        {
            [Required]
            [Display(Name = "Firstname")]
            public string FirstName { get; set; }

            [Display(Name = "Lastname")]
            public string LastName { get; set; }

            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The username is less than 100 characters.")]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [Display(Name = "User Role")]
            public string UserRole { get; set; }

            [Required]
            [Display(Name = "Status")]
            public string Status { get; set; }
        }
        public async Task OnGetAsync(string id)
        {
            var userInfo = await _userManager.FindByIdAsync(id);
            ViewData["Id"] = id;
            ViewData["UserFirstName"] = userInfo.FirstName;
            ViewData["UserLastName"] = userInfo.LastName;
            ViewData["UserName"] = userInfo.UserName;
            ViewData["Email"] = userInfo.Email;
            ViewData["PhoneNumber"] = userInfo.PhoneNumber;
            ViewData["Status"] = userInfo.Status == true ? "Active" : "Disabled";
            ViewData["Role"] = _userManager.GetRolesAsync(userInfo).Result.FirstOrDefault();

            // return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            bool curStatus = false;
            if (Request.Form["Status"].ToString() == "yes")
            {
                curStatus = true;
            }
            var user = await _userManager.FindByIdAsync(id);
            if (User.Identity.Name != user.UserName)
            {
                var result = await _userManager.DeleteAsync(user);
            }

            StatusMessage = "Successfully Deleted!";
            return Redirect("/Administrator/ManageAccounts");
        }
    }
}
