namespace Mock.Domain.Models;

public class TokenInformationModel
{
    public string Sub { get; set; } = null!;

    public DateTime NotValidBefore { get; set; }

    public DateTime ExpirationTime { get; set; }

    public DateTime IssuedAt { get; set; }

    public string? Issuer { get; set; }

    public string? Aud { get; set; }
}