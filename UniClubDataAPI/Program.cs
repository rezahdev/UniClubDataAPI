using Serilog;
using UniClubDataAPI.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
Log.Logger = new LoggerConfiguration().MinimumLevel.Warning().WriteTo.
    File("log/errorLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Services.AddDbContext<ApplicationDBContext>(option => {
    option.UseMySQL(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddControllers(option => { 
    //option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

builder.Host.UseSerilog();


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
