using Microsoft.EntityFrameworkCore;
using TweetsAPI.Data;
using TweetsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddApiVersioning(action =>
{
    action.DefaultApiVersion = new ApiVersion(1, 0);
    action.AssumeDefaultVersionWhenUnspecified = true;
    action.ReportApiVersions = true;
    action.ApiVersionReader = new HeaderApiVersionReader("c-api-version");
}
);

//SQL context
builder.Services.AddDbContextPool<TweetsDataContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn"));
});

// Services/repositories
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITweetsRepository, TweetsRepository>();

//Token authantocation serivice registration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateActor = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
