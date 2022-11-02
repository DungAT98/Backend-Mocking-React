namespace Mock.Domain.Dto;

public class TokenDto
{
    public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public DateTime RefreshTokenExpiredIn { get; set; }
}