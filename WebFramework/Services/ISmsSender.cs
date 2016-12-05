using System.Threading.Tasks;

namespace WebFramework.Services
{
    /// <summary>
    /// 短信发送服务接口
    /// </summary>
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
