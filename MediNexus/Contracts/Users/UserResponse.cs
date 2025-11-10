namespace MediNexus.Api.Contracts.Users
{
    public record UserResponse(int Id, string Name, string Email, string Role, bool IsActive, DateTime CreatedAt);
}
