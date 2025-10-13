using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;
using MVC_Lab_1.Repo;

namespace MVC_Lab_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IEntityRepo<Department>, DepartmentRepo>();
            builder.Services.AddScoped<IEntityRepo<Student>, StudentRepo>();
            builder.Services.AddScoped<IStudentEmailExist, StudentRepo>();
            builder.Services.AddDbContext<ITIDbContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("con1"));
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
