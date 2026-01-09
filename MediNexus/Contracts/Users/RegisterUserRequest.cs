namespace MediNexus.Api.Contracts.Users
{
    public record CreateUserRequest(
        int DocumentTypeId,
        string DocumentNumber,
        string FirstName,
        string LastName,
        string Username,
        string Email,
        int UserProfileId,
        int UserRoleId,
        int UserStatusId,
        string Password
    );

}
