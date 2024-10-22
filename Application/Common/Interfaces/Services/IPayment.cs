using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Shared.Common;
using Shared.DTOs.Payments;

namespace Application.Common.Interfaces.Services
{
    public interface IPayment
    {
        Task<PaymentSessionResult> InitiatePaymentSessionAsync(Order order);
        PaymentStatus ResolvePaymentStatusFromCallback(TelrPaymentCallbackDto dto);
    }
}
