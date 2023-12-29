using ImageHubAPI.Data;
using ImageHubAPI.Interfaces;
using ImageHubAPI.IService;
using ImageHubAPI.Models;
using ImageHubAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<IImageHubContext, ImageHubContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerBuilder =>
{
  swaggerBuilder.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "ImageHubAPI",
    Description = "Image Hub API for managing saving and viewing images",
  });

  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

  swaggerBuilder.EnableAnnotations();
  swaggerBuilder.IncludeXmlComments(xmlPath);

  swaggerBuilder.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "bearer"
  });

  swaggerBuilder.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type=ReferenceType.SecurityScheme,
          Id="Bearer"
        }
      },
      new string[]{}
    }
  });
});

builder.Services.AddIdentityCore<User>()
  .AddEntityFrameworkStores<ImageHubContext>();

builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserFriendRepository<User>, UserFriendRepository>();
builder.Services.AddScoped<IUserImgRepository<User>, UserImgRepository>();
builder.Services.AddScoped<IFriendshipRepository<Friendship>, FriendshipRepository>();
builder.Services.AddScoped<IDirectory, DirectoryService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,

      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
    };
  })
  .AddCookie(IdentityConstants.ApplicationScheme, options =>
  {
    options.LoginPath = "/api/Account/Login";
    options.AccessDeniedPath = "/api/Account/Login";
  });

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("UserAccess", policy =>
  {
    policy.RequireAuthenticatedUser();
    policy.RequireClaim("UserID");
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
