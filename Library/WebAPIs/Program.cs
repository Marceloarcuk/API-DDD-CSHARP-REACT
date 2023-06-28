using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Generics;
using Domain.Interfaces.InterfaceServices;
using Domain.Services;
using Entities.Entities;
using Intrastructure.Configuration;
using Intrastructure.Repository.Generics;
using Intrastructure.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Token;
using WebAPIs.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-------------------------------------------
// CONEXAO COM BANCO DE DADOS
//-------------------------------------------
builder.Services.AddDbContext<ContextBase>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextBase>();
//-------------------------------------------
// CONFIGURANDO OUTROS SERVIÇOS - NAO USADOS NESTE PROJETO
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//-------------------------------------------

//-------------------------------------------
// INTERLIGAR INTERFACE REPOSITORIO - SINGLETON
//-------------------------------------------
builder.Services.AddSingleton(typeof(IGenerics<>), typeof(RepositoryGenerics<>));
builder.Services.AddSingleton<IMessage, RepositoryMessage>();
//SERVICOS - DOMINIO
builder.Services.AddSingleton<IServiceMessage, ServiceMessage>();
//-------------------------------------------


//-------------------------------------------
// AUTENTICACAO COM O TOKEN
//-------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(option =>
             {
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,

                     ValidIssuer = "Teste.Securiry.Bearer",
                     ValidAudience = "Teste.Securiry.Bearer",
                     IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
                 };

                 option.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = context =>
                     {
                         Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                         return Task.CompletedTask;
                     }
                 };
             });
//--------------------------------------------

//-------------------------------------------
// AUTOMAPPER
//-------------------------------------------
var config = new AutoMapper.MapperConfiguration(
    cfg =>
    {
        cfg.CreateMap<MessageViewModel, Message>();
        cfg.CreateMap<Message, MessageViewModel>();
    });
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
//-------------------------------------------



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//-------------------------------------------
// CORS
//-------------------------------------------
/*
var urlDev = "https://localhost:7225/";
var urlHml = "https://localhost:7225/";
var urlPrd = "https://localhost:7225/";
app.UseCors(b => b.WithOrigins(urlDev, urlHml, urlPrd));
*/
var devClient = "https://localhost:7225/";
app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithOrigins(devClient));
//-------------------------------------------


app.UseHttpsRedirection();

//-------------------------------------------
// Autenticação do webtoken
//-------------------------------------------
app.UseAuthentication();
//-------------------------------------------
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();
app.Run();
