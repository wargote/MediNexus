namespace MediNexus.Api.Contracts.Users
{
    public record RegisterUserRequest(string Name, string Email, string Password, string Role);
}
