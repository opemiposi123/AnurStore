using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Domain.Entities;
using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ApplicationContext _context;

        public ReceiptRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Reciept> GenerateReceiptAsync(Reciept reciept)
        {
            var result = await _context.Reciepts.AddAsync(reciept);
            await _context.SaveChangesAsync();
            return reciept;
        }

        public async Task<IList<Reciept>> GetAllReceiptsAsync()
        {
            var receipt = await _context.Reciepts
                .Include(p => p.RecieptItems)
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
            return receipt;
        }

        public async Task<Reciept> GetReceiptByIdAsync(string id)
        {
            var receipt = await _context.Reciepts
                 .Include(p => p.RecieptItems)
                 .FirstOrDefaultAsync(p => p.Id == id);
            return receipt;
        }

        public async Task<bool> UpdateReceiptAsync(Reciept reciept)
        {
            _context.Reciepts.Update(reciept);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
