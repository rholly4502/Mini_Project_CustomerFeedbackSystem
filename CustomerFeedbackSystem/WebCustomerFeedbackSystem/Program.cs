using Microsoft.EntityFrameworkCore;
using WebCustomerFeedbackSystem.EF;
using WebCustomerFeedbackSystem.Models;
using WebCustomerFeedbackSystem.Service;

var builder = WebApplication.CreateBuilder(args);
// Registrasi DbContext
builder.Services.AddDbContext<CustomerFeedbackSystemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7090")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

//builder.Services.AddControllers();

builder.Services.AddScoped<IFeedback, FeedbackEF>();
// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddHttpClient<ServiceAPI>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == "POST" && context.Request.Form["_method"] == "PUT")
//    {
//        context.Request.Method = "PUT";
//    }
//    await next();
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
