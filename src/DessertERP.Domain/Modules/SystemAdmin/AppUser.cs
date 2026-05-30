using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.SystemAdmin;

public enum UserStatus { Active, Inactive, Locked }

public class AppUser : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Username     { get; private set; } = string.Empty;
    public string Email        { get; private set; } = string.Empty;
    public string FullName     { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserStatus Status   { get; private set; } = UserStatus.Active;
    public DateTime? LastLoginAt { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public string? RefreshToken          { get; private set; }
    public DateTime? RefreshTokenExpiry  { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private AppUser() { }

    public AppUser(Guid organizationId, string username, string email, string fullName, string passwordHash)
    {
        OrganizationId = organizationId;
        Username       = username.ToLowerInvariant().Trim();
        Email          = email.ToLowerInvariant().Trim();
        FullName       = fullName.Trim();
        PasswordHash   = passwordHash;
    }

    public void UpdateProfile(string email, string fullName)
    {
        Email    = email.ToLowerInvariant().Trim();
        FullName = fullName.Trim();
        SetUpdated();
    }

    public void SetPasswordHash(string hash) { PasswordHash = hash; SetUpdated(); }

    public void RecordLogin()
    {
        LastLoginAt        = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        LockedUntil        = null;
        SetUpdated();
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        if (FailedLoginAttempts >= 5)
        {
            Status      = UserStatus.Locked;
            LockedUntil = DateTime.UtcNow.AddMinutes(15);
        }
        SetUpdated();
    }

    public void Unlock()   { Status = UserStatus.Active; FailedLoginAttempts = 0; LockedUntil = null; SetUpdated(); }
    public void Deactivate() { Status = UserStatus.Inactive; SetUpdated(); }
    public void Activate()   { Status = UserStatus.Active;   SetUpdated(); }

    public bool IsLockedOut => Status == UserStatus.Locked ||
                               (LockedUntil.HasValue && LockedUntil > DateTime.UtcNow);

    public void SetRefreshToken(string token, DateTime expiry)
    {
        RefreshToken       = token;
        RefreshTokenExpiry = expiry;
        SetUpdated();
    }

    public void ClearRefreshToken() { RefreshToken = null; RefreshTokenExpiry = null; SetUpdated(); }

    public void AssignRole(Guid roleId)
    {
        if (_userRoles.Any(r => r.RoleId == roleId)) return;
        _userRoles.Add(new UserRole(Id, roleId));
        SetUpdated();
    }

    public void RemoveRole(Guid roleId)
    {
        var ur = _userRoles.FirstOrDefault(r => r.RoleId == roleId);
        if (ur != null) { _userRoles.Remove(ur); SetUpdated(); }
    }
}
