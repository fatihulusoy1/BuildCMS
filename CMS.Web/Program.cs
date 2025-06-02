var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ApiBaseUrl config ayar� (appsettings.json veya ortam de�i�keninden okunuyor)
string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001/"; // Default fallback

// HttpClient ve PostService kayd�
builder.Services.AddHttpClient<CMS.Web.Services.PostService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<CMS.Web.Services.PostService>();

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
