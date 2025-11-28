var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient for n8n integration
builder.Services.AddHttpClient("n8n", client =>
{
    var baseUrl = builder.Configuration["N8N:BaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("N8N:BaseUrl is not configured in appsettings.json");
    }
    
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Assistant}/{action=Index}/{id?}");

// Legacy route for backward compatibility
app.MapControllerRoute(
    name: "legacy-ask",
    pattern: "Workflow/Ask",
    defaults: new { controller = "Assistant", action = "Index" });

app.Run();
