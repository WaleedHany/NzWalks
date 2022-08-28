// creates a web application builder class which is then
// used to inject the dependencies into the services collection
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using NzWalks.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation
  (options => options.RegisterValidatorsFromAssemblyContaining<Program>());

// // Inject DbContext class into the services collection
builder.Services.AddDbContext<NzWalksDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalks"));
});

// // Inject repositories
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();
// // Inject a profile mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
// Finally we run the application
app.Run();
