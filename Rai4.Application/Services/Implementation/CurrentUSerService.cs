using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Rai4.Application.Services.Interfaces;

namespace Rai4.Application.Services.Implementation;

public class CurrentUSerService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdString != null ? int.Parse(userIdString) : throw new Exception("User is not authenticated");
        }
    }
}