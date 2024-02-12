using ImageHubAPI.Data;
using ImageHubAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ImageHubAPI.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckUserExistsFilter : ActionFilterAttribute
    {
        private readonly ImageHubContext? _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CheckUserExistsFilter(ImageHubContext? context)
        {
            _context = context;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("uploadImgDto", out var dto))
            {
                var imgDto = (UploadImgDto)dto;

                if (!await _context.Users.AnyAsync(u => u.Id == imgDto.UserID))
                {
                    context.Result = new NotFoundObjectResult($"User with ID:{imgDto.UserID} not exist");
                }
            }
            base.OnActionExecuting(context);
        }
    }

    public class CheckUserExistsAttribute : TypeFilterAttribute
    {
        public CheckUserExistsAttribute()
            : base(typeof(CheckUserExistsFilter)) { }
    }
}
