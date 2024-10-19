using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// controller
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Optional
    // options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()); // Add this if using a converter
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// controller

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsStudent", policy =>
        policy.RequireClaim("IsSale", "True"));

    options.AddPolicy("IsAdmin", policy =>
        policy.RequireClaim("IsAdmin", "True"));
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
// builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
//     options.Password.RequireDigit = true;
//     options.Password.RequiredLength = 8;
//     options.Password.RequireUppercase = true;
//     options.Password.RequireNonAlphanumeric = false;
// }).AddEntityFrameworkStores<ApplicationDBContext>()
//     .AddDefaultTokenProviders();

// builder.Services.AddAuthentication(options => {
//     options.DefaultAuthenticateScheme = 
//     options.DefaultChallengeScheme =
//     options.DefaultForbidScheme =
//     options.DefaultScheme = 
//     options.DefaultSignInScheme = 
//     options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
   
// }).AddJwtBearer(options =>{
//     options.TokenValidationParameters = new TokenValidationParameters{
//         ValidateIssuer = true,
//         ValidIssuer = builder.Configuration["JWT:Issuer"],
//         ValidateAudience = true,
//         ValidAudience = builder.Configuration["JWT:Audience"],
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(
//             System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
//         )

//     };
// });
// //User-Setting and JWT Authentication



// //database
// builder.Services.AddDbContext<ApplicationDBContext>(options => {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
// });
// //database

// builder.Services.AddHangfire(config => 
//     config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DatabaseConnection")));
// builder.Services.AddHangfireServer();

// builder.Services.AddScoped<IJWTService, JWTServices>();
// builder.Services.AddScoped<IAdminRepository, AdminRepository>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

