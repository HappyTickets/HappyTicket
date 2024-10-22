using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Payments;

namespace Application.Payments.Service
{
    internal class PaymentService: IPaymentService
    {
        private readonly IUnitOfWork _ufw;
        private readonly IPayment _payment;

        public PaymentService(IUnitOfWork ufw, IPayment payment)
        {
            _ufw = ufw;
            _payment = payment;
        }

        public async Task<BaseResponse<Empty>> ProcessPaymentCallbackAsync(TelrPaymentCallbackDto dto)
        {
            var orderRepo = _ufw.Repository<Order>();
            var order = (await orderRepo.GetByIdAsync(dto.Tran_CartId))!;

            var paymentStatus = _payment.ResolvePaymentStatusFromCallback(dto);

            switch (paymentStatus)
            {
                case PaymentStatus.OnHold:
                    order.PaymentStatus = PaymentStatus.OnHold;
                    break;
                case PaymentStatus.Authorized:
                    await HandleAuthorizedPaymentAsync(order);
                    break;
                case PaymentStatus.Declined:
                    order.PaymentStatus = PaymentStatus.Declined;
                    break;
                case PaymentStatus.Cancelled:
                    order.PaymentStatus = PaymentStatus.Cancelled;
                    break;
                case PaymentStatus.Refunded:
                    order.PaymentStatus = PaymentStatus.Refunded;
                    break;
            }

            // set order tran_ref
            order.PaymentTranRef = dto.Tran_Ref;
            orderRepo.Update(order);
            await _ufw.SaveChangesAsync();

            return Empty.Default;
        }

        private async Task HandleAuthorizedPaymentAsync(Order order)
        {
            var cartRepo = _ufw.Repository<Cart>();
            var cart = await cartRepo.FirstOrDefaultAsync(c=>c.UserId == order.UserId,
                [
                    nameof(Cart.CartItems),
                    string.Join(".", [nameof(Cart.CartItems), nameof(CartItem.Ticket)])
                ]);

            foreach (var cartItem in cart!.CartItems!)
                cartItem.Ticket.TicketStatus = TicketStatus.Sold;

            order.PaymentStatus = PaymentStatus.Authorized;
            cart.CartItems.Clear();
            cartRepo.Update(cart);
        }
    }
}
