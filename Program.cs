using Custom_Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.UseWorkingHours();
app.UseMiddleware<WorkingHourMiddleware>();

app.MapGet("/", () => "Welcome, You are with in Working Hours");

app.Run();
