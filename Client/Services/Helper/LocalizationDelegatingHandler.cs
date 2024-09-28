
using System.Globalization;

namespace Client.Services.Helper
{
    public class LocalizationDelegatingHandler : HttpClientHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(currentCulture.Name));
            return base.Send(request, cancellationToken);
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(currentCulture.Name));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
