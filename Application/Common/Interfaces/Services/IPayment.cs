using Domain.Entities;
using Shared.Common;

namespace Application.Common.Interfaces.Services
{
    public interface IPayment
    {
        Task ProcessOrderPaymentAsync(Order order);
    }
}
