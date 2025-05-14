using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Week5Lab.Models; // ApplicationUser ve Ceng382DbContext burada
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// SQL bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Server=MERT-MONSTER\\SQLEXPRESS;Database=Ceng382DB;Trusted_Connection=True;TrustServerCertificate=True;";

// DbContext'e bağla
builder.Services.AddDbContext<Ceng382DbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Sadece bu Identity tanımı kullanılacak
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<Ceng382DbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

// Razor Pages ve Session
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware'ler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session middleware

app.UseAuthentication(); // Identity login kontrol
app.UseAuthorization();  // Role & yetki kontrol

app.MapRazorPages();

app.Run();
