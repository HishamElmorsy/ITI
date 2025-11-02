using ApricotChat.Data;
using ApricotChat.Services;
using Microsoft.EntityFrameworkCore;
using ApricotChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<GroqOptions>(builder.Configuration.GetSection("Groq"));
builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
builder.Services.AddHttpClient<IGroqService, GroqService>();
builder.Services.AddHttpClient<GroqService>();
builder.Services.AddHttpClient<OllamaService>();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddSingleton<IModelRouter, ModelRouter>();
builder.Services.AddTransient<IModelService, GroqService>();
builder.Services.AddTransient<ClaudeService>();
builder.Services.AddTransient<MetaLlamaService>();
builder.Services.AddTransient<DeepSeekService>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Ensure database is created (no migrations required initially). Swap to Migrate() after adding migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ChatHub>("/hubs/chat");


app.Run();
