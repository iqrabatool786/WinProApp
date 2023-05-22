using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WinProApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace WinProApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Administrator")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<WinProAppUser> _userManager;
        //private readonly WinProAppContext _Context;

        public UsersModel(UserManager<WinProAppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public List<UsersViewModel> CurUsers { get; set; }

        
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {

                var userInfo = await _userManager.Users.ToListAsync();

                List<UsersViewModel> users = new List<UsersViewModel>();

                    foreach(var item in userInfo)
                    {
                        var curUser = await _userManager.FindByIdAsync(item.Id);
                        users.Add(new UsersViewModel
                        {
                            Id = item.Id,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            UserName = item.UserName,
                            PhoneNumber = item.PhoneNumber,
                            Status = item.Status == true ? "Active" : "Disable",
                            Role = _userManager.GetRolesAsync(curUser).Result.Count() > 1? (_userManager.GetRolesAsync(curUser).Result[1] + " - " + _userManager.GetRolesAsync(curUser).Result[0]): _userManager.GetRolesAsync(curUser).Result.FirstOrDefault()
                        });
                    }

                CurUsers = users;

                return Page();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
