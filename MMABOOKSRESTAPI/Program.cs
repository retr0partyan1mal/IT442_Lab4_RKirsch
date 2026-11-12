using MMABooksEFClasses;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>

    {
        builder.AllowAnyOrigin().WithMethods("POST", "PUT", "DELETE", "GET", "OPTIONS").AllowAnyHeader();
    });
});

// Connect to MySQL database
string connectionString = ConfigDB.GetMySqlConnectionString();

builder.Services.AddDbContext<MMABooksContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 28))
    );
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();