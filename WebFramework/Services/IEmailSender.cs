using System.Threading.Tasks;

namespace WebFramework.Services
{
    /// <summary>
    /// 邮件发送服务接口
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
