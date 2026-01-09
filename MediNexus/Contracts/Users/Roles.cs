namespace MediNexus.Api.Contracts.Users
{
    public record UserRoleResponse(
        int Id,
        string Name,
        string? Description,
        bool IsActive
    );

    public record CreateUserRoleRequest(
        string Name,
        string? Description
    );

    public record UpdateUserRoleRequest(
        string Name,
        string? Description,
        bool IsActive
    );
}
