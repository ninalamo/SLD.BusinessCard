using System.Security.Claims;
using BusinessCard.Domain.Seedwork;

namespace BusinessCard.API;

public class CurrentUser : ICurrentUser
{
    private readonly bool _isAuthenticated;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        Name = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        IdentityId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        Roles = httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?.Select(i => i.Value).ToArray();

        var data = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.UserData);
        if (data != default)
        {
            _company = data.Split("-")[0];
            _prcLicense = data.Split("-")[1];
        }
        
        _isAuthenticated = !string.IsNullOrEmpty(Email);
    }

    public string Name { get; }
    public string[] Roles { get; }
    public string Email { get; }
    public string IdentityId { get; }


    public bool IsAdmin()
    {
        return Roles.Contains("ADMIN");
    }

    public bool IsBroker()
    {
        return Roles.Contains("BROKER");
    }

    private readonly string _company;
    private readonly string _prcLicense;
    public string GetCompany() => _company;
    public string GetPrcLicense() => _prcLicense;
}