using AnurStore.Domain.Entities;
using System.Threading.Tasks;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IReceiptRepository
    {
        Task<Reciept> GenerateReceiptAsync(Reciept reciept);
        Task<IList<Reciept>> GetAllReceiptsAsync();
        Task<Reciept> GetReceiptByIdAsync(string id);
        Task<bool> UpdateReceiptAsync(Reciept reciept);
    }
}
