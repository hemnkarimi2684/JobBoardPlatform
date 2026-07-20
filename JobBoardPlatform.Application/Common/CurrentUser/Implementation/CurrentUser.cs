using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.Application.Common.CurrentUser.Implementation;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => FindSub(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);


    // این کد رو برای این زدم که بره از توی توکنی که صادر شده کلیم های فقط از نوع رول رو بگیره و مقدار ولیو شون رو بهم بده 
    public List<string> UserRoles => _httpContextAccessor.HttpContext?.User?
                                                                         .FindAll(ClaimTypes.Role)
                                                                         .Select(c => c.Value)
                                                                         .ToList() ?? new List<string>();

    private Guid FindSub(string value)
    {
        Guid.TryParse(value, out var subId);

        return subId;
    }
}
