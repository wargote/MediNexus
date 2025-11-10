namespace MediNexus.Api.Contracts.Auth
{
    public record AuthResponse(string AccessToken, DateTime ExpiresAt);
}
