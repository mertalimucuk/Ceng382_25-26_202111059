var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//SESSION EKLENTİSİ 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Session 20 dakika sürsün
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// AI PROMPT: How Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Authentication varsa  önce bunu eklet
app.UseAuthentication();

//Session aktif et
app.UseSession();

// Yetkilendirme
app.UseAuthorization();

app.MapRazorPages();

app.Run();
