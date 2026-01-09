namespace MediNexus.Api.Contracts.Users
{
    public record UserResponse(
        int Id,
        int DocumentTypeId,
        string? DocumentTypeName,
        string DocumentNumber,
        string FirstName,
        string LastName,
        string Username,
        string Email,
        int UserProfileId,
        string? UserProfileName,
        int UserRoleId,
        string? UserRoleName,
        int UserStatusId,
        string? UserStatusName,
        bool IsActive,
        DateTime CreatedAt
    );


    public record UpdateUserRequest(
    int DocumentTypeId,
    string DocumentNumber,
    string FirstName,
    string LastName,
    string Username,
    int UserProfileId,
    int UserRoleId,
    int UserStatusId,
    string Email,
    bool IsActive
    );
}
