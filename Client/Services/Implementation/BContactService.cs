using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.ContactDto;

namespace Client.Services.Implementation
{
    public class BContactService : BIContactService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public BContactService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        public async Task<Result<BaseResponse<bool>>> SendMessageAsync(Contact contactModel, CancellationToken cancellationToken = default)
        {
            return await _httpClientHelper.PostBaseAsync<Contact, BaseResponse<bool>>("api/Contact/SendMessage", contactModel);
        }

    }
}
