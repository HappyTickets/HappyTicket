namespace Shared.Common.General
{
    public static class UrlHelper
    {
        private static string BlazorBase  = string.Empty;

        private static string APIBase = string.Empty;

        public static void SetCommunicationUrls(string blazorBaseUrl, string apiBaseUrl)
        {
            BlazorBase = blazorBaseUrl;
            APIBase = apiBaseUrl;
        }

        public static string GetBlazorBase()
        {
            return BlazorBase;
        }

        public static string GetAPIBase()
        {
            return APIBase;
        }
    }
}
