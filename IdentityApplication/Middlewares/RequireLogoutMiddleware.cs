namespace IdentityApplication.Middlewares
{
    public class RequireLogoutMiddleware
    {
        private readonly RequestDelegate _next;

        public RequireLogoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var requestUrl = $"{context.Request.Path}{context.Request.QueryString}";
                if (requestUrl.Contains("/Identity/Account/Login"))
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 302;
                    context.Response.Headers.Add("Location", "/User/Logout");
                    return;
                }
            }

            await _next(context);
        }
    }

}
