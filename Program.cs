using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CombustionCarGuideWeb.Data;
using CombustionCarGuideWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 ตั้งค่า ConnectionString (แก้ให้ตรงกับของคุณ)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);




// ✅ ลงทะเบียน Identity Services
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // ปิดการยืนยันอีเมลก่อนเข้าสู่ระบบ
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// ✅ เปิดใช้งาน Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// ✅ เปิดใช้งาน Controllers และ Razor Pages
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ กำหนด Middleware ต่างๆ
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ✅ รองรับการใช้งาน Static Files เช่น CSS, JS, Images

app.UseRouting();



app.UseSession(); // ✅ เปิดใช้งาน Session Middleware
app.UseAuthentication(); // ✅ เปิดใช้งาน Authentication Middleware
app.UseAuthorization(); // ✅ เปิดใช้งาน Authorization Middleware

// ✅ ตั้งค่า Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // ✅ รองรับ Razor Pages (สำหรับ Identity)

app.Run();
