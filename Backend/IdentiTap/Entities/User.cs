namespace IdentTap.Domain.Entities;

public class User
{
    public int UserID { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? MicrosoftID { get; set; }
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}