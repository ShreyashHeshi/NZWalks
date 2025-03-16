using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()               // debug,information, warning,error
    .CreateLogger();


builder.Logging.ClearProviders();  // this will clear out any provider that we have injected till now
builder.Logging.AddSerilog(logger);



builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor(); // to find folder file location in image handling

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks Api", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,   // ParameterLocation from Microsoft.OpenApi.Models
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header

            },
            new List<string>()
        }
    });
    
});

builder.Services.AddDbContext<NZWalksDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepositary,SQLRegionRepositary>();
builder.Services.AddScoped<IWalkRepositary,SQLWalkRepositary>();
builder.Services.AddScoped<ITokenRepositary,TokenRepositary>();
builder.Services.AddScoped<IImageRepositary,LocalImageRepositary>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// set up roles and users identity along with jwt for users
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()  // takes type of NZWalksAuthDbContext
    .AddDefaultTokenProviders();    // this used for reset pass and change email


// options here for pass that we want to configure
builder.Services.Configure<IdentityOptions>(options =>     // configure takes IdentityOptions as a action
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //  JwtBearerDefaults from Microsoft.AspNetCore.Authentication.JwtBearer
                                                                           // AuthenticationScheme for jwt authentication which default value= Bearer
     .AddJwtBearer(options =>
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"], // it read it from appsetting.json for list of valid issuer and valid audience
         ValidAudience = builder.Configuration["Jwt:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
         // SymmetricSecurityKey comes from Microsoft.IdentityModel.Tokens used for handling authentication and authorization
     });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider= new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    RequestPath="/Images"
    // https://localhost:1234/Images
});

app.MapControllers();

app.Run();
