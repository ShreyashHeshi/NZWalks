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
using NZWalks.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// webapplication is class which provide static createbuilder method
// webapplication is static class defined in Microsoft.AspNetCore.Builder


// Add services to the container.

var logger = new LoggerConfiguration()                                         // LoggerConfiguration is class from serilog pakage
    .WriteTo.Console()                                                         // serilog.sinks.console
    .WriteTo.File("Logs/NzWalks_Log.txt",rollingInterval: RollingInterval.Day) //rollingInterval creates new file after day where exception logged in txt file. Minute option also there
    .MinimumLevel.Warning()                                                    // debug,information, warning,error
    .CreateLogger();                                                           // creates the logger instance


builder.Logging.ClearProviders();    // this will clear out in build logging provider
builder.Logging.AddSerilog(logger);  // logging is instance of ILoggingBuilder from Microsoft.Extensions.Logging



builder.Services.AddControllers();
// Services is instance of IServiceCollection
// AddController() is extension method for Microsoft.Extensions.DependencyInjection

builder.Services.AddHttpContextAccessor(); // helps to use httpcontext outside controller like handling image path

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks Api", Version = "v1" });            // OpenApiInfo from Microsoft.OpenApi.Models.OpenApiInfo
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,                                                               // ParameterLocation from Microsoft.OpenApi.Models
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme 
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement   // Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {                                                           
            new OpenApiSecurityScheme                               //Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference=new OpenApiReference                      // Microsoft.OpenApi.Models.OpenApiReference
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

app.UseMiddleware<ExceptionHandlerMiddleware>();

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
