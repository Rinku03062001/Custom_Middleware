


using System.Net.NetworkInformation;

namespace Custom_Middleware
{
    public class WorkingHourMiddleware
    {
        private readonly RequestDelegate _next;
        public WorkingHourMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentHour = DateTime.Now.Hour;
            var currentMinute = DateTime.Now.Minute;

            if(currentHour >= 9 && currentHour <= 17)
            {
                await _next(context);
            }
            else if(currentHour == 5 && currentMinute == 0 )
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "text/html";

                await context.Response.WriteAsync($@"
                    <html>
                        <body style='font-family:sans-serif; text-align:center; padding-top:50px;'>
                            <h2>Access Denied</h2>
                            <p>Current time: <strong>{DateTime.Now:hh:mm tt}</strong></p>
                            <p>Allowed working hours are from <strong>9:00 AM</strong> to <strong>12:00 PM</strong></p>
                        </body>
                    </html>");

                //await context.Response.WriteAsync("Access Denied, Outside of Working Hours(9 AM - 5 PM).");
            }
        }
    }

    public static class WorkingHoursMiddlewareExtensions
    {
        public static IApplicationBuilder UseWorkingHours(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WorkingHourMiddleware>();
        }
    }
}
