namespace DessertERP.Application.Common.Interfaces;

/// <summary>
/// Resolves the active Organization for the current HTTP request.
/// The Angular front-end sends the selected org via the X-Organization-Id header.
/// </summary>
public interface ICurrentOrganizationService
{
    /// <summary>
    /// Returns the OrganizationId from the current request context.
    /// Returns Guid.Empty when no org header is present (e.g. seeding, health-checks).
    /// </summary>
    Guid OrganizationId { get; }

    /// <summary>
    /// True when a valid org id has been resolved for the current request.
    /// </summary>
    bool HasOrganization => OrganizationId != Guid.Empty;
}
