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
using System.Text.Json.Serialization;

// Thêm namespace cho Google Authentication
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SWP391_CareSkin_BE
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) Load appsettings.json
            // (Đã được load mặc định trong CreateBuilder)

            // 2) Load googleauth.json (nếu có)
            var googleAuthPath = Path.Combine(Directory.GetCurrentDirectory(), "googleauth.json");
            if (File.Exists(googleAuthPath))
            {
                builder.Configuration.AddJsonFile(googleAuthPath, optional: false, reloadOnChange: true);
            }

            // Đọc file facebookkey.json
            var facebookAuthPath = Path.Combine(Directory.GetCurrentDirectory(), "facebookkey.json");
            if (File.Exists(facebookAuthPath))
            {
                builder.Configuration.AddJsonFile(facebookAuthPath, optional: false, reloadOnChange: true);
            }

            // Lấy connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Error: Connection String not created. Check again appsettings.json!");
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

            // Add HttpClient Factory for Facebook/Google API calls
            builder.Services.AddHttpClient();

            // CORS
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

            // JWT
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];
            var jwtKey = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT configuration is missing. Please check appsettings.json or environment variables.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            builder.Services.AddSingleton<JwtHelper>();

            // Đăng ký các DI Services, Repositories
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

            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IFAQRepository, FAQRepository>();
            builder.Services.AddScoped<IFAQService, FAQService>();

            builder.Services.AddScoped<IVnpayRepository, VnpayRepository>();
            builder.Services.AddScoped<IVnpayService, VnpayService>();

            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();

            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();

            builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            builder.Services.AddScoped<IResultRepository, ResultRepository>();
            builder.Services.AddScoped<IResultService, ResultService>();

            builder.Services.AddScoped<IProductDetailIngredientRepository, ProductDetailIngredientRepository>();
            builder.Services.AddScoped<IProductForSkinTypeRepository, ProductForSkinTypeRepository>();
            builder.Services.AddScoped<IProductMainIngredientRepository, ProductMainIngredientRepository>();
            builder.Services.AddScoped<IProductUsageRepository, ProductUsageRepository>();
            builder.Services.AddScoped<IProductVariationRepository, ProductVariationRepository>();
            builder.Services.AddScoped<IUserQuizAttemptRepository, UserQuizAttemptRepository>();
            builder.Services.AddScoped<IUserQuizAttemptService, UserQuizAttemptService>();

            builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
            builder.Services.AddScoped<IFirebaseService, FirebaseService>();

            // Rating & Feedback
            builder.Services.AddScoped<IRatingFeedbackRepository, RatingFeedbackRepository>();
            builder.Services.AddScoped<IRatingFeedbackImageRepository, RatingFeedbackImageRepository>();
            builder.Services.AddScoped<IRatingFeedbackService, RatingFeedbackService>();

            // Routine & RoutineProduct
            builder.Services.AddScoped<IRoutineRepository, RoutineRepository>();
            builder.Services.AddScoped<IRoutineService, RoutineService>();
            builder.Services.AddScoped<IRoutineProductRepository, RoutineProductRepository>();
            builder.Services.AddScoped<IRoutineProductService, RoutineProductService>();
            builder.Services.AddScoped<IRoutineStepRepository, RoutineStepRepository>();
            builder.Services.AddScoped<IRoutineStepService, RoutineStepService>();

            builder.Services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
            builder.Services.AddScoped<IBlogNewsService, BlogNewsService>();

            // Hangfire
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
            builder.Services.AddHangfireServer();

            // PromotionUpdaterJob
            builder.Services.AddScoped<PromotionUpdaterJob>();

            // Controllers & JSON Options
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 3) Lấy Google Client ID & Secret từ googleauth.json
            var googleClientId = builder.Configuration["GoogleAuth:ClientId"];
            var googleClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];

            // Có thể kiểm tra nếu chưa khai báo -> thông báo hoặc bỏ qua
            if (string.IsNullOrEmpty(googleClientId) || string.IsNullOrEmpty(googleClientSecret))
            {
                Console.WriteLine("Warning: Google ClientId/ClientSecret is missing. Check googleauth.json if you need Google OAuth.");
            }

            // Lấy Facebook AppId & AppSecret
            var facebookAppId = builder.Configuration["FacebookAuth:AppId"];
            var facebookAppSecret = builder.Configuration["FacebookAuth:AppSecret"];
            if (string.IsNullOrEmpty(facebookAppId) || string.IsNullOrEmpty(facebookAppSecret))
            {
                Console.WriteLine("Warning: Facebook AppId/AppSecret is missing. Check facebookkey.json if needed.");
            }

            // 4) Cấu hình Authentication (JWT & Google)
            builder.Services.AddAuthentication(options =>
            {
                // Mặc định dùng JWT cho xác thực API
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Google sẽ được dùng khi user nhấn đăng nhập
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Google cần cookie để xử lý đăng nhập
            })
            .AddCookie() // Cookie Authentication để hỗ trợ Google login
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
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["GoogleAuth:ClientId"];
                options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.CallbackPath = "/signin-google";
                options.SaveTokens = true;
            })
            .AddFacebook(options =>
            {
                options.AppId = facebookAppId;
                options.AppSecret = facebookAppSecret;
                options.Scope.Add("email");
                options.Fields.Add("email");
                options.Fields.Add("name");
                options.Fields.Add("picture");
                options.CallbackPath = "/signin-facebook";
                options.SaveTokens = true;
            });

            var app = builder.Build();

            // Tự động migrate DB
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

            // Migrate lần nữa (nếu cần)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                dbContext.Database.Migrate();
            }

            // Middleware
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // Hangfire Dashboard
            app.UseHangfireDashboard();

            // Schedule recurring job để update promotion statuses hằng ngày lúc nửa đêm
            RecurringJob.AddOrUpdate<PromotionUpdaterJob>(
                "update-promotion-statuses",
                job => job.UpdatePromotionStatusesAsync(),
                "0 0 * * *"
            );

            // Kiểm tra missed job khi app khởi động
            using (var scope = app.Services.CreateScope())
            {
                var promotionUpdaterJob = scope.ServiceProvider.GetRequiredService<PromotionUpdaterJob>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                var now = DateTime.Now;
                var lastMidnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                var jobStorage = JobStorage.Current;
                using (var connection = jobStorage.GetConnection())
                {
                    var recurringJob = connection.GetRecurringJobs(new[] { "update-promotion-statuses" }).FirstOrDefault();
                    if (recurringJob != null)
                    {
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
