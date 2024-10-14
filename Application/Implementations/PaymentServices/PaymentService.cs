using Application.Interfaces;
using Application.Interfaces.PaymentServices;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QRCoder;
using Shared.Common.General; // Ensure to include this namespace
using Shared.DTOs.PaymentDTOs;
using Shared.Extensions;
using Shared.ResourceFiles;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace Application.Implementations.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


        public PaymentService(IUnitOfWork unitOfWork,
                              IEmailSender emailSender,
                              IFileService fileService,
                              ILogger<PaymentService> logger,
                              IMapper mapper,
                              UserManager<ApplicationUser> userManager,
                              IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _fileService = fileService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
        }

        public async Task<Result<PaymentResponseDto>> SendPaymentRequestAsync(PaymentRequestDto paymentRequestDto)
        {
            var result = new PaymentResponseDto();

            try
            {
                var userResult = await _userManager.FindByIdAsync(paymentRequestDto.UserId);

                if (userResult == null)
                {
                    result.HasErrors = true;
                    result.Errors.Add("Failed to retrieve order or user!");
                    return result;
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(PaymentConfiguration.HttpClientBaseAddress);
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;

                    string auth = $"{PaymentConfiguration.StoreId}:{PaymentConfiguration.AuthKey}";
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(auth);
                    auth = Convert.ToBase64String(data);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");

                    var authorizedToken = Guid.NewGuid().ToString();
                    var declinedToken = Guid.NewGuid().ToString();
                    var cancelledToken = Guid.NewGuid().ToString();

                    var requestBody = JsonSerializer.Serialize(new
                    {
                        method = "create",
                        store = PaymentConfiguration.StoreId,
                        authkey = PaymentConfiguration.AuthKey,
                        framed = PaymentConfiguration.Framed,
                        language = PaymentConfiguration.Language,
                        ivp_applepay = "1",
                        order = new
                        {
                            cartid = paymentRequestDto.CartId,
                            test = PaymentConfiguration.Test,
                            amount = paymentRequestDto.Amount,
                            currency = PaymentConfiguration.Currency,
                            description = PaymentConfiguration.Description,
                            trantype = "sale"
                        },
                        customer = new
                        {
                            Ref = paymentRequestDto.UserId,
                            email = userResult.Email,
                            name = new
                            {
                                forenames = userResult.NormalizedUserName,
                                surname = "Z"
                            },
                            address = new
                            {
                                line1 = PaymentConfiguration.Line1,
                                city = PaymentConfiguration.City,
                                country = PaymentConfiguration.Country
                            },
                            phone = userResult.PhoneNumber
                        },
                        Return = new
                        {
                            authorised = $"{PaymentConfiguration.AuthorisedUrl}{authorizedToken}",
                            declined = $"{PaymentConfiguration.DeclinedUrl}{declinedToken}",
                            cancelled = $"{PaymentConfiguration.CancelledUrl}{cancelledToken}"
                        }
                    });

                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var responseJson = await httpClient.PostAsync(PaymentConfiguration.PostUrl, content);

                    if (responseJson.IsSuccessStatusCode)
                    {
                        var responseContent = await responseJson.Content.ReadAsStringAsync();

                        var response = JsonSerializer.Deserialize<DeserializedPaymentResponse>(responseContent, JsonSerializerOptions);

                        var orderDetails = response?.Order;

                        if (!string.IsNullOrWhiteSpace(orderDetails?.Url))
                        {
                            result.PaymentUrl = orderDetails?.Url;
                            result.PaymentRef = orderDetails?.Ref;
                            result.AuthorizedToken = authorizedToken;
                            result.DeclinedToken = declinedToken;
                            result.CancelledToken = cancelledToken;
                        }
                        else
                        {
                            result.HasErrors = true;
                            result.Errors.Add($"Payment failed! {response?.Error}");
                        }
                    }
                    else
                    {
                        result.HasErrors = true;
                        result.Errors.Add($"Payment request failed with status code {responseJson.StatusCode}!");
                    }
                }
            }
            catch (Exception ex)
            {
                result.HasErrors = true;
                result.Errors.Add($"An error occurred {ex.Message}");
                _logger.LogError(ex, "Error in SendPaymentRequestAsync endpoint!");
            }

            return await result.ToResultAsync();
        }


        public async Task<PaymentStatusDto> CheckPaymentStatusAsync(Guid orderId)
        {
            var result = new PaymentStatusDto();

            try
            {
                var orderResult = await _unitOfWork.Repository<OrderO>().GetByIdAsync(orderId);

                var order = orderResult.Match(
                    succ => succ,      // Success: Get the Order object
                    fail => null!      // Failure: Return null
                );

                if (order == null)
                {
                    result.HasErrors = true;
                    result.Errors.Add("Failed to retrieve order!");
                    return result;
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(PaymentConfiguration.HttpClientBaseAddress);
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;

                    string auth = $"{PaymentConfiguration.StoreId}:{PaymentConfiguration.AuthKey}";
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(auth);
                    auth = Convert.ToBase64String(data);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");

                    var request = new
                    {
                        method = "check",
                        store = PaymentConfiguration.StoreId,
                        authkey = PaymentConfiguration.AuthKey,
                        order = new
                        {
                            Ref = order.PaymentOrderRef
                        }
                    };

                    var requestBody = JsonSerializer.Serialize(request);

                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var responseJson = await httpClient.PostAsync(PaymentConfiguration.PostUrl, content);

                    if (responseJson.IsSuccessStatusCode)
                    {
                        var responseContent = await responseJson.Content.ReadAsStringAsync();

                        var response = JsonSerializer.Deserialize<DeserializedPaymentResponse>(responseContent, JsonSerializerOptions);

                        order.PaymentStatus = (int)PaymentConfiguration.PaymentStatusEnum.Paid; // Left for now!
                        _unitOfWork.Repository<OrderO>().Update(order);
                        await _unitOfWork.SaveChangesAsync();

                        result.HasErrors = false;
                        result.StatusText = nameof(PaymentConfiguration.PaymentStatusEnum.Paid); // Left for now!
                    }
                    else
                    {
                        result.HasErrors = true;
                        result.Errors.Add($"Payment request failed with status code {responseJson.StatusCode}!");
                    }
                }
            }
            catch (Exception ex)
            {
                result.HasErrors = true;
                result.Errors.Add($"An error occurred {ex.Message}");
                _logger.LogError(ex, "Error in CheckPaymentStatusAsync");
            }

            return result;
        }


        public async Task<Result<Unit>> SendTicketEmailAsync(string email, ICollection<CartItem> ticketItems, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Building Ticket Email.");
                List<Attachment> emailAttachments = [];
                StringBuilder ticketsEmailBody = new();
                foreach (var ticketItem in ticketItems)
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketItem.Ticket?.Id.ToString() ?? string.Empty, QRCodeGenerator.ECCLevel.H);
                    PngByteQRCode pngqrCode = new PngByteQRCode(qrCodeData);
                    var pngQrCode = pngqrCode.GetGraphic(7);
                    var savePath = $"/Images/User/{email}/Tickets/";
                    emailAttachments.Add(new Attachment(new MemoryStream(pngQrCode), $"{ticketItem.Ticket?.Match?.TeamA?.Name} vs {ticketItem.Ticket?.Match?.TeamB?.Name} - {ticketItem.Ticket?.Class} Ticket.png", "image/png"));
                    var qrPath = await SaveTicketQR(pngQrCode, savePath);
                    var qrUri = new Uri(new Uri(UrlHelper.GetAPIBase()), qrPath).ToString();

                    var pngImg = $"<a href=\"{qrUri}\"><img src=\"{qrUri}\" alt=\"Ticket QR-Code\" title=\"Ticket QR-Code\" width=\"100\" height=\"100\" border=\"0\" style=\"border:0; outline:none; text-decoration:none; display:block;\" /></a>";

                    ticketsEmailBody = ticketsEmailBody.Append(Resource.Invoice_Email_Table_Body_Template.Replace("{HomeTeam}", ticketItem.Ticket?.Match?.TeamA?.Name)
                                                                                                         .Replace("{AwayTeam}", ticketItem.Ticket?.Match?.TeamB?.Name)
                                                                                                         .Replace("{YourTeam}", ticketItem.Ticket?.Team?.Name)
                                                                                                         .Replace("{Stadium}", ticketItem.Ticket?.Match?.Stadium?.Name)
                                                                                                         .Replace("{Date}", ticketItem.Ticket?.Match?.EventDate.ToString())
                                                                                                         .Replace("{Class}", ticketItem.Ticket?.Class)
                                                                                                         .Replace("{TicketNumber}", ticketItem.Ticket?.Id.ToString())
                                                                                                         .Replace("{QR}", pngImg));
                }
                var emailContent = Resource.Invoice_Email_Template.Replace("{body}", ticketsEmailBody.ToString());
                _logger.LogInformation("Sending Ticket Email.");
                var result = await _emailSender.SendEmailAsync(email, Resource.Invoice_Title, emailContent.Replace("{style}", Resource.Invoice_Email_Style), emailAttachments, cancellationToken);
                _logger.LogInformation("Successfully Sent Ticket Email.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email Error.");
                return new(ex);
            }
        }

        private async Task<string> SaveTicketQR(byte[] base64, string path)
        {
            try
            {
                _logger.LogInformation("Saving the QR code.");
                var result = await _fileService.ConvertBase64ToFileAsync(base64, path);
                _logger.LogInformation("Successfully Saved the QR code.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't save the QR code");
                return string.Empty;
            }
        }
    }
}
