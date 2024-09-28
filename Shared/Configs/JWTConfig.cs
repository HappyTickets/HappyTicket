namespace Shared.Configs;

public class JWTConfig
{
    public string? Secret { get; set; }
    public TimeSpan DefaultTokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);
}
