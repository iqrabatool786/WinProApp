using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace WinProApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the WinProAppUser class
public class WinProAppUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    [PersonalData]
    public bool Status { get; set; }

    [PersonalData]
    public int StoreId { get; set; }
}

