//using Microsoft.AspNetCore.Authorization;
//using School.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace School.Authorization
//{
//    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, FileData>
//    {
//        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, FileData resource)
//        {
//            if(requirement.ResourceOperation == ResourceOperation.Read ||
//               requirement.ResourceOperation == ResourceOperation.Create)
//            {
//                context.Succeed(requirement);
//            }

//            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
//            if (resource.UploadedBy == userId)
//            {
//                context.Succeed(requirement);
//            }
//            return Task.CompletedTask;
//        }
//    }
//}
