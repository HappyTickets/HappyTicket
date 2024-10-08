namespace API.Extensions
{
    public static class CORsServiceExtension
    {
        public static void AddCORsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.AllowAnyOrigin()//WithOrigins(UrlHelper.GetBlazorBase())
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }
    }
}
