using Microsoft.EntityFrameworkCore;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Application.ServicesImplementation;
using TicketEase.Configurations;
using TicketEase.Mapper;
using TicketEase.Persistence.Context;
using TicketEase.Persistence.Extensions;
using TicketEase.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
var services = builder.Services;
var env = builder.Environment;


// Add services to the container.
//builder.Services.AddHttpClient();
//builder.Services.AddCloudinaryService(config);
//builder.Services.AddMailService(config);




// Authentication configuration
builder.Services.AddDbContext<TicketEaseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketEaseConnection"))
);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBoardServices, BoardServices>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddAuthentication();
builder.Services.AuthenticationConfiguration(configuration);

// Identity  configuration
builder.Services.IdentityConfiguration();
builder.Services.AddLoggingConfiguration(builder.Configuration);




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Ease v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
