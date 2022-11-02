namespace Mock.Domain.Configurations;

public class JwtConfiguration
{
    public const string Jwt = nameof(Jwt);

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public string Key { get; set; } = default!;
}