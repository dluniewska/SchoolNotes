using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IUserContextService
    {
        int GetUserId { get; }
        ClaimsPrincipal User { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
        public int GetUserId => int.Parse(User is null ? null : User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
