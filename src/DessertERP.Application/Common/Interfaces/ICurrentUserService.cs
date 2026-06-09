namespace DessertERP.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string Username { get; }
    string? IpAddress { get; }
}
