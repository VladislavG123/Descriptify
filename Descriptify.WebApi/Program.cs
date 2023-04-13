using System.Text;
using Descriptify;
using Descriptify.Bll;
using Descriptify.Contracts.Options;
using Descriptify.Dal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBusinessLoginLayout();
builder.Services.AddDataAccessLayout();

builder.Services.Configure<SecretOptions>(builder.Configuration.GetSection("SecretOptions"));

builder.Services.AddDbContext<ApplicationContext>(x => 
    x.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemory")!));

#region Jwt Configuration

var secrets = builder.Configuration.GetSection("SecretOptions");

var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret")!);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

#endregion

ConfigureServicesSwagger.ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();