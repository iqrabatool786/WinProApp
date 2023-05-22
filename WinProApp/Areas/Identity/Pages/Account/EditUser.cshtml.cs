using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WinProApp.Areas.Identity.Data;

namespace WinProApp.Areas.Identity.Pages.Account
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<WinProAppUser> _userManager;
        private readonly ILogger<EditUserModel> _logger;

        public EditUserModel(
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

            [Required]
            [Display(Name = "StoreId")]
            public int StoreId { get; set; }
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
            ViewData["Status"] = userInfo.Status == true?"yes":"no";
            ViewData["StoreId"] = userInfo.StoreId;
            ViewData["Role"] = "";// _userManager.GetRolesAsync(userInfo).Result.FirstOrDefault();
            var assignRoles = _userManager.GetRolesAsync(userInfo).Result.ToList();
            ViewData["AdditionalRole"] = "";

            if (assignRoles.Contains("Administrator"))
            {
                ViewData["Role"] = "Administrator";
            }
            if (assignRoles.Contains("CRM"))
            {
                ViewData["Role"] = "CRM";
            }
            if (assignRoles.Contains("HRMS"))
            {
                ViewData["Role"] = "HRMS";
            }
            if (assignRoles.Contains("Finance"))
            {
                ViewData["Role"] = "Finance";
            }
            if (assignRoles.Contains("POS"))
            {
                ViewData["Role"] = "POS";
            }
            if (assignRoles.Contains("Warehouse"))
            {
                ViewData["Role"] = "Warehouse";
            }
            if (assignRoles.Contains("Purchase"))
            {
                ViewData["Role"] = "Purchase";
            }
            if (assignRoles.Contains("Woocommerce"))
            {
                ViewData["Role"] = "Woocommerce";
            }
            if (assignRoles.Contains("Projects"))
            {
                ViewData["Role"] = "Projects";
            }
            if (assignRoles.Contains("Operations"))
            {
                ViewData["Role"] = "Operations";
            }
            if (assignRoles.Contains("Manager"))
            {
                ViewData["AdditionalRole"] = "Manager";
            }
            if (assignRoles.Contains("User"))
            {
                ViewData["AdditionalRole"] = "User";
            }


            // return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            bool curStatus = false;
            if(Request.Form["Status"].ToString() == "yes")
            {
                curStatus = true;
            }

            string strStoreId = Request.Form["StoreId"].ToString();
            string curRole = Request.Form["UserRole"].ToString();
            string oldRole = Request.Form["oldRole"].ToString();
            string additionalRole = Request.Form["Permission"].ToString();
            

            var user = await _userManager.FindByIdAsync(id);
            var assignRoles = _userManager.GetRolesAsync(user).Result.ToList();
            user.Id= id;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Email= Input.Email;
            user.PhoneNumber = Input.PhoneNumber;
            user.Status = curStatus;
            user.StoreId = int.Parse(strStoreId);
            await _userManager.UpdateAsync(user);

            if(oldRole != "" && oldRole != curRole || (!assignRoles.Contains(additionalRole)))
            {
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (assignRoles.Contains("Manager"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Manager");
                }
                if (assignRoles.Contains("User"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "User");
                }
                await _userManager.AddToRoleAsync(user, curRole);

                if (additionalRole == "Manager")
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }
                if (additionalRole == "User")
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
            }
            

            StatusMessage = "Successfully Updated!";
            return Redirect("/Administrator/ManageAccounts");
        }
    }
}
