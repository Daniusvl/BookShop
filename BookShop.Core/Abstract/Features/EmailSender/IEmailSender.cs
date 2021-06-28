using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Features.EmailSender
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string html_message);
    }
}
