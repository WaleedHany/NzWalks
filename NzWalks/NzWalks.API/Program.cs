// creates a web application builder class which is then
// used to inject the dependencies into the services collection
using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using NzWalks.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// // Inject DbContext class into the services collection
builder.Services.AddDbContext<NzWalksDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalks"));
});

// // Inject repositories
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
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
