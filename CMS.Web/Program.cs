using CMS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ApiBaseUrl config ayari (appsettings.json veya ortam degiskeninden okunuyor)
string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001/"; // Default fallback

// HttpClient ve PostService kaydi
builder.Services.AddHttpClient<CMS.Web.Services.PostService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<CMS.Web.Services.PostService>();

// HttpClient ve CompanyService kaydi
builder.Services.AddHttpClient<CompanyService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<CompanyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();