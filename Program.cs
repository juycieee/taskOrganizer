using TaskOrganizer.Models;
using TaskOrganizer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// --- MongoDB Configuration and Services ---

// Configure MongoDB Settings from appsettings.json
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// ✨ UPDATED: Registering Services with Scoped lifetime
// (EmployeeService is the new name for the old UserService)
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<AdminService>();    // ✨ NEW: Registration for Admin Collection
builder.Services.AddScoped<EmailService>();

// --- Application Pipeline ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Redirect root → Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();
app.Run();