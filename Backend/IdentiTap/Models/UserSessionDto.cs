
namespace IdentTap.Models;

public class UserSessionDto
{
    public int UserID { get; set; }
    public string Email { get; set; } = string.Empty;
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public record MicrosoftLoginRequest(string IdToken);
public record StandardLoginRequest(string Email, string Password);