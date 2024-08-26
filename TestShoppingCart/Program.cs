
using Microsoft.EntityFrameworkCore;
using TestShoppingCart.Data;
using TestShoppingCart.Interfaces;
using TestShoppingCart.Repositories;

namespace TestShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // เพิ่มบริการสำหรับ Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            // เพิ่มการตั้งค่า CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // แก้ไข URL นี้ให้ตรงกับ frontend ของคุณ
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials(); // อนุญาตการส่งคำขอพร้อมกับ Cookies
                    });
            });

            builder.Services.AddDbContext<ShoppingCartContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStockRepository, StockRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowFrontend");
            // ใช้งาน Session ก่อนการใช้งาน middleware อื่น ๆ ที่ต้องพึ่งพา session
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
