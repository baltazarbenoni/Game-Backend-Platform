using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Application.Services;
using Application.Interfaces;
using Infrastructure.Repositories;
using Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Db connection
string GetConnectionString()
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if(connectionString != null) return connectionString;
    else throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(GetConnectionString()));
#endregion

#region Controllers 
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<RegistrationValidator>();
builder.Services.AddScoped<LoginValidator>();
#endregion

#region Build
var app = builder.Build();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

