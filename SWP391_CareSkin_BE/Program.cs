using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Repositories;
using SWP391_CareSkin_BE.Services;
using Hangfire;
using Hangfire.SqlServer;
using SWP391_CareSkin_BE.Jobs;
using Microsoft.Extensions.Logging;
using Hangfire.Storage;

namespace SWP391_CareSkin_BE
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Error: Connection String not create. Check again appsettings.json!");
            }

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,           
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorNumbersToAdd: null
        )
    )
);


            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "http://careskinbeauty.shop")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });


            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];
            var jwtKey = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT configuration is missing. Please check appsettings.json or environment variables.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            builder.Services.AddSingleton<JwtHelper>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = key
                    };
                });


            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IProductPictureRepository, ProductPictureRepository>();
            builder.Services.AddScoped<IProductPictureService, ProductPictureService>();

            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddScoped<IStaffRepository, StaffRepository>();
            builder.Services.AddScoped<IStaffService, StaffService>();

            builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
            builder.Services.AddScoped<IPromotionService, PromotionService>();

            builder.Services.AddScoped<ISkinTypeRepository, SkinTypeRepository>();
            builder.Services.AddScoped<ISkinTypeService, SkinTypeService>();

            builder.Services.AddScoped<IFAQRepository, FAQRepository>();
            builder.Services.AddScoped<IFAQService, FAQService>();

            builder.Services.AddScoped<IVnpayRepository, VnpayRepository>();
            builder.Services.AddScoped<IVnpayService, VnpayService>();


            builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();

            builder.Services.AddScoped<IFirebaseService, FirebaseService>();
            
            // Register Rating & Feedback services and repositories
            builder.Services.AddScoped<IRatingFeedbackRepository, RatingFeedbackRepository>();
            builder.Services.AddScoped<IRatingFeedbackImageRepository, RatingFeedbackImageRepository>();
            builder.Services.AddScoped<IRatingFeedbackService, RatingFeedbackService>();

            // Configure Hangfire
            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add Hangfire server
            builder.Services.AddHangfireServer();

            // Register the PromotionUpdaterJob
            builder.Services.AddScoped<PromotionUpdaterJob>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware Configuration

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.Migrate();
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure Hangfire dashboard
            app.UseHangfireDashboard();

            // Schedule recurring job to update promotion statuses daily at midnight
            RecurringJob.AddOrUpdate<PromotionUpdaterJob>(
                "update-promotion-statuses",
                job => job.UpdatePromotionStatusesAsync(),
                "0 0 * * *");

            // Check if we need to run a missed job when the application starts
            using (var scope = app.Services.CreateScope())
            {
                var promotionUpdaterJob = scope.ServiceProvider.GetRequiredService<PromotionUpdaterJob>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                
                // Get the current time
                var now = DateTime.Now;
                var lastMidnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                
                // If the app is started after midnight but before the next scheduled run,
                // and if the job hasn't run today yet, run it immediately
                var jobStorage = JobStorage.Current;
                using (var connection = jobStorage.GetConnection())
                {
                    var recurringJob = connection.GetRecurringJobs(new[] { "update-promotion-statuses" }).FirstOrDefault();
                    
                    if (recurringJob != null)
                    {
                        // Check if the job has run today
                        var lastExecution = recurringJob.LastExecution;
                        
                        if (lastExecution == null || lastExecution.Value.Date < now.Date)
                        {
                            logger.LogInformation("Detected missed promotion update job. Running it now...");
                            BackgroundJob.Enqueue(() => promotionUpdaterJob.UpdatePromotionStatusesAsync());
                        }
                    }
                }
            }

            app.MapControllers();

            app.Run();
        }
    }
}
