namespace MediNexus.Api.Contracts.Users
{
    public record UserProfileResponse(
        int Id,
        string Name,
        string? Description,
        int UserRoleId,
        string? NameRol,
        bool IsActive
    );

    public record CreateUserProfileRequest(
        string Name,
        string? Description,
        int UserRoleId
    );

    public record UpdateUserProfileRequest(
        string Name,
        string? Description,
        bool IsActive,
        int UserRoleId
    );
}
