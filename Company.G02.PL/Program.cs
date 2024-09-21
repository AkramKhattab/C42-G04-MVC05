using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using Company.G02.PL.Mapping.Employees;
using Company.G02.PL.Services;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //builder.Services.AddScoped<AppDbContext>(); // Allow Dependancy Injection For AppDbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For AppDbContext


            

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow DI For DepartmentRepository            
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();  / Allow DI For EmployeeRepository

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));



            // Life Time           
            //builder.Services.AddScoped();     // Per Request Becomes an UnReachable Object
            //builder.Services.AddSingleton();  // Per App , Caching
            //builder.Services.AddTransient();  // Per Operations , Mapping

            builder.Services.AddScoped<IScopedService, ScopedService > ();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

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
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();







        }
    }
}