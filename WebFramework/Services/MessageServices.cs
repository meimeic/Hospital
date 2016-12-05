using System.Threading.Tasks;

namespace WebFramework.Services
{
    /// <summary>
    /// 当identity启用双重认证时，应用程序发送认证信息
    /// </summary>
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
