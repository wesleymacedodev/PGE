using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PGE.Interfaces;
using PGE.Mappings;
using PGE.Models;
using PGE.Repositories;
using PGE.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var configuration = builder.Configuration;

// Conexão com banco de dados
builder.Services.AddDbContext<PGEContext>(
    options => {
        options.UseSqlServer(configuration["Settings:SqlConnectionString"]);
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(configuration["Settings:CORSorigin"])
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();

// Configurando swagger para receber token de autenticação
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configurando o jwt 

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Settings:JWTIssuer"],
        ValidAudience = configuration["Settings:JWTAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Settings:JWTIssuerKey"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Definindo o escopo das interfaces e dos repositorios para o contexto
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAuthenticate, AuthenticateService>();
builder.Services.AddScoped<IProcessoRepository, ProcessoRepository>();
builder.Services.AddScoped<IDocumentoRepository, DocumentoRepository>();
builder.Services.AddScoped<IDistribuirRepository, DistribuirRepository>();

// Definindo o mapeamento das classes para o dto
builder.Services.AddAutoMapper(typeof(EntitiesToDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Definir autenticacao e autorizacao

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
