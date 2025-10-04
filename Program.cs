using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.System.Text.Json;
using StackExchange.Redis.Extensions.Core.Configuration;
using UrlShortener.Data;

var builder = WebApplication.CreateBuilder(args);
// In Program.cs
//builder.Services.AddSingleton<UrlShortener.Data.IUrlShortener, UrlShortener.Data.RedisUrlShortener>();
builder.Services.AddScoped<IUrlShortener, RedisUrlShortener>();
builder.Services.AddScoped<IApplyShorteningStrategy, Md5BasedUrlShorteningStrategy>();
builder.Services.AddSingleton<ValidateAntiForgeryTokenAttribute>();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? string.Empty));

// Add services to the container.

// Without AddControllersWithViews(), we get an error stating that the ValidateAntiForgeryTokenAuthorizationFilter service
// was not registered
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(new RedisConfiguration()
{
    ConnectionString = builder.Configuration.GetConnectionString("Redis") 
    ?? throw new InvalidOperationException("Missing Redis connection string in appsettings.json")
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapStaticAssets();


//app.MapControllerRoute(
//        name: "default",
//       pattern: "{controller=UrlShortener}/{action=Index}/{shortUrlKey?}")
//  .WithStaticAssets();


app.Run();