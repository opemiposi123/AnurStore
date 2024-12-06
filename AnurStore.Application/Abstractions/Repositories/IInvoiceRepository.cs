using AnurStore.Domain.Entities;
using System.Threading.Tasks;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GenerateInvoice(Invoice invoice); 
        Task<IList<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(string id);
        Task<bool> UpdateInvoice(Invoice invoice);
        List<Invoice> GetAllInvoice();
    }
}
