using AspNetMvcCacheRedis.Services.Implements;
using AspNetMvcCacheRedis.Services.Interfaces;
using StackExchange.Redis;

namespace AspNetMvcCacheRedis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string redisConnStr = builder.Configuration.GetSection("Redis")["ConnectionString"];
            var redis = ConnectionMultiplexer.Connect(redisConnStr);
            builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
