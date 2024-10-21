using Application.Common.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Payment.Models._PaymentRequest;
using Infrastructure.Payment.Models._PaymentResponse;
using Infrastructure.Payment.Models.PaymentRequest._PaymentRequest;
using Infrastructure.Payment.Models.PaymentRequest.PaymentRequest;
using Microsoft.Extensions.Options;
using Shared.Common;
using System.Net.Http.Json;

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

        public async Task ProcessOrderPaymentAsync(Order order)
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
                   //Trantype = "sale"
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

            order.PaymentUrl = result.Order.Url;
            order.PaymentOrderRef = result.Order.Ref;
        }
    }
}
