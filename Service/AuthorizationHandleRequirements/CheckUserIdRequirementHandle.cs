using ImageHubAPI.DTOs;
using ImageHubAPI.Service.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;

namespace ImageHubAPI.Service.AuthorizationHandleRequirements
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckUserIdRequirementHandle :
        AuthorizationHandler<CheckUserIdRequirement, AddFriendDto?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <param name="resource"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckUserIdRequirement requirement, AddFriendDto? resource)
        {
            var userIdClaim = context?.User?.FindFirst("UserID")?.Value;
            if (userIdClaim != null && userIdClaim == resource.UserId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }


    }
}
