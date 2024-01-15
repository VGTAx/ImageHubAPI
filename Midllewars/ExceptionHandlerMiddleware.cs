namespace ImageHubAPI.Midllewars
{
  /// <summary>
  /// 
  /// </summary>
  public class ExceptionHandlerMiddleware
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (Exception)
      {
        await HandleExceptionAsync(context);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task HandleExceptionAsync(HttpContext context)
    {
      context.Response.StatusCode = 500;
      await context.Response.WriteAsync("An unexpected error occurred.");
    }

  }

  /// <summary>
  /// 
  /// </summary>
  public static class ExceptionHandlerMiddlewareExtension
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
      return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
  }
}
