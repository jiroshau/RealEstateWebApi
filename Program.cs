using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using RealEstateWebApi.Data;



var builder = WebApplication.CreateBuilder(args);


var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite");


builder.Services.AddDbContext<RealEstateContext>(options =>
{
    if (useSqlite)
    {
        var connection = new SqliteConnection("Data Source=C:\\Users\\Acer\\source\\repos\\RealEstateWebApi\\RealEstateDB.db");
        connection.Open();

        
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        options.UseSqlite(connection);
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection"));
    }
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
    try
    {
        var usersCount = context.users.Count();
        Console.WriteLine($"Users count in database: {usersCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database test failed: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
    context.Database.EnsureCreated();
}

app.Run();
