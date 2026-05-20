namespace NextHorizont.Models;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}

public class ErrorResponse
{
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}
