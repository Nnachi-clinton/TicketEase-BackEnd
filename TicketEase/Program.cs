using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketEase.Common.Utilities;
using TicketEase.Configurations;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;
using TicketEase.Persistence.Extensions;
using TicketEase.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
var services = builder.Services;
var env = builder.Environment;

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddAutoMapper(typeof(TicketEase.Mapper.MapperProfile));

builder.Services.AddAuthentication();
builder.Services.AuthenticationConfiguration(configuration);

// Identity  configuration
builder.Services.IdentityConfiguration();
builder.Services.AddLoggingConfiguration(builder.Configuration);
//builder.Services.AddTransient<Seeder>();
builder.Services.AddMailService(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContext<TicketEaseDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionStrings")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TicketEaseDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("TicketEase")));

builder.Services.AddSwagger();

//builder.Services.AddIdentity<AppUser, IdentityRole>()
//			   .AddEntityFrameworkStores<TicketEaseDbContext>()
//			   .AddDefaultTokenProviders();
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

