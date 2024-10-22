using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Payment.Models._PaymentRequest;
using Infrastructure.Payment.Models._PaymentResponse;
using Infrastructure.Payment.Models.Common;
using Infrastructure.Payment.Models.PaymentRequest._PaymentRequest;
using Infrastructure.Payment.Models.PaymentRequest.PaymentRequest;
using Microsoft.Extensions.Options;
using Shared.DTOs.Payments;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Payment
{
    internal class TelrPaymentService : IPayment
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelrPaymentSettings _settings;

        public TelrPaymentService(IHttpClientFactory httpClientFactory, IOptions<TelrPaymentSettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        public async Task<PaymentSessionResult> InitiatePaymentSessionAsync(Order order)
        {
           // prepare payment request
           var request = new PaymentRequest
           {
               Method = "create",
               Store = _settings.StoreId,
               AuthKey = _settings.AuthKey,
               Framed = _settings.Framed,
               Order = new OrderRequest
               {
                   CartId = order.Id.ToString(),
                   Test = _settings.Test,
                   Amount = order.TotalAmount.ToString(),
                   Currency = _settings.Currency,
                   Description = "Tickets Payment",
                   Trantype = "sale"
               },
               Return = new ReturnRequest
               {
                   Authorised = _settings.AuthorisedUrl,
                   Declined = _settings.DeclinedUrl,
                   Cancelled = _settings.CancelledUrl,
               },
               Customer = new CustomerRequest
               {
                   Ref = order.UserId.ToString(),
                   Email = order.User.Email!,
                   Phone = order.User.PhoneNumber!,
                   Name = new NameRequest
                   {
                       Forenames = order.User.UserName!,
                       Surname = "_x_"
                   },
                   Address = new AddressRequest
                   {
                       Line1 = "5 th street, address line 234, next address",
                       City = "Riyadh",
                       Country = "SA"
                   }
               }
           };

            // send request
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsJsonAsync("https://secure.telr.com/gateway/order.json", request);
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("Create session failure.");

            var result = (await response.Content.ReadFromJsonAsync<PaymentResponse>())!;

            if (result.Error != null)
                throw new Exception("Create session failure.");

            return new PaymentSessionResult
            {
                PaymentUrl = result.Order.Url,
                OrderRef = result.Order.Ref
            };
        }

        public PaymentStatus ResolvePaymentStatusFromCallback(TelrPaymentCallbackDto dto)
        {
            if (!IsCallbackValid(dto))
                throw new Exception("Invalid callback.");

            switch (dto.Tran_Type)
            {
                case TransactionTypes.Sale:
                case TransactionTypes.Capture:
                    if (dto.Tran_Status == TransactionStatus.Authorized)
                    {
                        return PaymentStatus.Authorized;
                    }
                    break;

                case TransactionTypes.Auth:
                    if (dto.Tran_Status == TransactionStatus.Hold)
                    {
                        return PaymentStatus.OnHold;
                    }
                    break;

                case TransactionTypes.Refund:
                    if (dto.Tran_Status == TransactionStatus.Authorized)
                    {
                        return PaymentStatus.Refunded;
                    }
                    break;

                case TransactionTypes.Release:
                    if (dto.Tran_Status == TransactionStatus.Authorized)
                    {
                        return PaymentStatus.Cancelled;
                    }
                    break;
            }

            return PaymentStatus.Declined;
        }

        private bool IsCallbackValid(TelrPaymentCallbackDto dto)
        {
            var unhashed_string = string.Join(":", [
                _settings.SecretKey,
                dto.Tran_Store,
                dto.Tran_Type,
                dto.Tran_Class,
                dto.Tran_Test,
                dto.Tran_Ref,
                dto.Tran_PrevRef,
                dto.Tran_FirstRef,
                dto.Tran_Currency,
                dto.Tran_Amount,
                dto.Tran_CartId,
                dto.Tran_Desc,
                dto.Tran_Status,
                dto.Tran_AuthCode,
                dto.Tran_AuthMessage
                ]);

            var hashed_bytes = SHA1.HashData(Encoding.UTF8.GetBytes(unhashed_string));
            var hashed_string = BitConverter.ToString(hashed_bytes).Replace("-", "").ToLowerInvariant();

            return hashed_string == dto.Tran_Check;
        }
    }
}
