namespace Mock.Domain.Dto;

public class TokenDto
{
    public string Token { get; set; } = default!;

    public DateTime? ExpiredIn { get; set; } 
}