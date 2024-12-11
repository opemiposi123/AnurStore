using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> CreateSupplier(Supplier supplier);
        Task<IList<Supplier>> GetAllSuppliers(); 
        Task<Supplier> GetSupplierById(string id); 
        Task<bool> UpdateSupplier(Supplier supplier);
        Task<bool> Exist(string supplierName);
        List<Supplier> GetAllSupplier();
    }
}
