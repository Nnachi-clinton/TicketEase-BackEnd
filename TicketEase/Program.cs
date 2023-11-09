using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Application.ServicesImplementation;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Application.ServicesImplementation;
using TicketEase.Common.Utilities;
using TicketEase.Configurations;
using TicketEase.Domain.Entities;
using TicketEase.Mapper;
using TicketEase.Persistence.Context;
using TicketEase.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
var services = builder.Services;
var env = builder.Environment;


//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IBoardServices, BoardServices>();
//builder.Services.AddScoped<ITicketService, TicketService>();
//builder.Services.AddScoped<ITicketRepository, TicketRepository>();
//builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddAuthentication();
builder.Services.AuthenticationConfiguration(configuration);
builder.Services.AddAutoMapper(typeof(Program));

// Identity  configuration
builder.Services.IdentityConfiguration();
builder.Services.AddLoggingConfiguration(builder.Configuration);
//builder.Services.AddTransient<Seeder>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContext<TicketEaseDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionStrings")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TicketEaseDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("TicketConnectionString")));

builder.Services.AddSwagger();

builder.Services.AddIdentity<AppUser, IdentityRole>()
			   .AddEntityFrameworkStores<TicketEaseDbContext>()
			   .AddDefaultTokenProviders();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDependencies(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Ease v1"));
}
using (var scope = app.Services.CreateScope())
{
	var serviceprovider = scope.ServiceProvider;
	Seeder.SeedRolesAndSuperAdmin(serviceprovider);
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

