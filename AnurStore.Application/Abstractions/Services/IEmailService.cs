using AnurStore.Application.RequestModel;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MailReceiverDto model, MailRequests request);
    }
}
