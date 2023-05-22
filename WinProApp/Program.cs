using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Globalization;
using WinProApp.Areas.Identity.Data;
using WinProApp.DataModels.DataBase;
using WinProApp.Extentions;
using WinProApp.Settings;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WinProAppContextConnection") ?? throw new InvalidOperationException("Connection string 'WinProAppContextConnection' not found.");
var emailSettings = builder.Configuration.GetSection("EMailSettings");


builder.Services.AddDbContext<WinProAppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<WinProDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WinProAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WinProAppContext>();

builder.Services.AddSession();
//Add Email Configurations
builder.Services.Configure<EmailSettings>(emailSettings);
builder.Services.AddTransient<WinProApp.Services.IMailService, WinProApp.Services.EmailService>();
//builder.Services.AddDomainServices();
// Add services to the container.
  ///  builder.Services.AddHttpContextAccessor();
builder.Services.AddDomainServices();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new List<CultureInfo> {
            new CultureInfo("en"),
            new CultureInfo("ar")
        };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

builder.Services.AddMvc().AddDataAnnotationsLocalization();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization()
        .AddMvcLocalization()
        .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.Configure<FormOptions>(options =>
        {
            options.ValueCountLimit = 1024 * 1024; // 1024 * 1024 items max
            options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
        });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Login";
    options.LogoutPath = "/Logout";
    options.SlidingExpiration = true;
});

var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseAuthentication(); ;

app.UseAuthorization();

app.UseSession();
//app.Use(async (context, next) =>
//{
//    string sUrlPath = context.Request.Path.Value;
//    string newUrl = "";
//    if (sUrlPath.Contains("Identity/Account/Login")){
//        newUrl ="/Login";
//        context.Response.Redirect(newUrl);
//    }
//   await next.Invoke();
//});

app.UseRewriter(
 new RewriteOptions().Add(context =>
 {
     if (context.HttpContext.Request.Path == "/Identity/Account/Login")
     {
         context.HttpContext.Response.Redirect("/Login");
     }
 }));



app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();
    app.Run();
