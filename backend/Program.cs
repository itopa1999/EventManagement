using System.Globalization;
using System.Text.Json.Serialization;
using backend.AdminRepository;
using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Repository;
using backend.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-NG");
var cultures = new[] { cultureInfo };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultureInfo);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

builder.Services.AddHttpContextAccessor();

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.File("Helpers/Logs/logfile.txt", rollingInterval: RollingInterval.Day)
//     .CreateLogger();

// builder.Host.UseSerilog();


// controller
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Optional
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    // options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()); // Add this if using a converter
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// controller



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy =>
        policy.RequireClaim("IsAdmin", "True"));

    options.AddPolicy("IsOrganizer", policy =>
        policy.RequireClaim("IsOrganizer", "True"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EventManagement APIs", Version = "v1" });

    // Add the JWT security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});



// //User-Setting and JWT Authentication
builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(40);
    options.Lockout.MaxFailedAccessAttempts = 4;
    options.Lockout.AllowedForNewUsers = true;

}).AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
   
}).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )

    };
});
// //User-Setting and JWT Authentication


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 700 * 1024; // Set limit to 700 KB
});

// // Database configuration
// builder.Services.AddDbContext<DBContext>(options => {
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"));
// });

// // Hangfire configuration
// builder.Services.AddHangfire(config => 
//     config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DatabaseConnection")));
// builder.Services.AddHangfireServer();



// //database
builder.Services.AddDbContext<DBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});
// //database

builder.Services.AddHangfire(config => 
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<SpecialHelpers>();


builder.Services.AddScoped<IJWTService, JWTServices>();
builder.Services.AddScoped<IAuthInterface, IAuthRepository>();
builder.Services.AddScoped<IOrganizerInterfaces, OrganizerRepository>();
builder.Services.AddScoped<IAttendeeInterface, AttendeeRepository>();
builder.Services.AddScoped<IAdminInterface, AdminRepository>();
builder.Services.AddHttpClient<FlutterwaveService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<SmtpSettingsDto>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<EmailService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});




var app = builder.Build();

app.UseHangfireDashboard();

// RecurringJob.AddOrUpdate<ReminderService>(
//     "send-reminders", 
//     service => service.CheckAndSendRemindersAsync(), 
//     Cron.Minutely);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseStaticFiles(); // Enable default static file handling

// Optional: Serve files from the "Media" folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Media")),
    RequestPath = "/Media"
});
        

app.UseCors("AllowAllOrigins");

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.GetUserAsync(context.User);

        if (user != null && (!user.EmailConfirmed || !user.PhoneNumberConfirmed))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Email or Phone number not confirmed.");
            return;
        }
    }
    await next.Invoke();
});
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BlockAllUserMiddleware>();

app.MapControllers();



app.Run();

