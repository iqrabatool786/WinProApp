using WinProApp.Models;

namespace WinProApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
