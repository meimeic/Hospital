using System.Threading.Tasks;

namespace WebFramework.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
