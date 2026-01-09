namespace MediNexus.Api.Contracts.Security
{
    public record HashRequest(string Password);
    public record HashResponse(string Hash);
}
