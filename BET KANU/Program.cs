using BET_KANU.Services;
using BetKanu.Data;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddRazorPages();
builder.Services.AddMvc();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BKdbContext>(options =>
           options.UseSqlServer(connectionstring));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BKdbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 8;
//    options.Password.RequireNonAlphanumeric = true;
//    options.User.RequireUniqueEmail = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    
    if (context.Request.Path == "/thenecklace/ep1")
    {
        // Get the dialect and ensure case-insensitive comparison
        var dialect = context.Request.Query["dialect"].ToString().ToLower();

        // Log or debug the received dialect
        Console.WriteLine($"Received dialect: {dialect}");

        switch (dialect)
        {
            case "eastern":
                context.Response.Redirect("https://www.lulu.com/shop/akkad-saadi-and-omar-issac-and-omar-issac-and-gabriella-gawrieh/the-necklace-eastern-surit-ep-1/paperback/product-rm8vz2n.html?q=+The+Necklace+-+Eastern", true);
                return;

            case "western":
                context.Response.Redirect("https://www.lulu.com/shop/akkad-saadi-and-omar-issac-and-gabriella-gawrieh-and-george-afram/the-necklace-western-surayt-ep-1/paperback/product-jezm5dr.html?q=the+necklace+-+western", true);
                return;

            default:
                // Log when no valid dialect is found
                Console.WriteLine($"No valid dialect found. Received: {dialect}");
                break;
        }
    }

    await next();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
   );

app.Run();
