namespace WebApiJWT.Models
{
    public record LoginResponse(string AccessToken,
    int? UserId,
    int? RoleId,
    string Username);
}
