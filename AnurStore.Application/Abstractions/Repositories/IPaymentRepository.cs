using AnurStore.Domain.Entities;

namespace AnurStore.Application.Abstractions.Repositories;

public interface IPaymentRepository
{
    Task<Payment> CreatePayment(Payment payment);
    Task<IList<Payment>> GetAllPayments();
    Task<Payment> GetPaymentById(string id);
    Task<bool> UpdatePayment(Payment payment);
    Task<bool> Exist(string paymentName); 
    List<Payment> GetAllPayment();
}
