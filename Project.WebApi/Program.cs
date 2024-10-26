using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Project.Business.DataProtection;
using Project.Business.Operations.User;
using Project.Data.Context;
using Project.Data.Repositories;
using Project.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DataProtector
builder.Services.AddScoped<IDataProtection, DataProtection>();
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));
builder.Services.AddDataProtection()
    .SetApplicationName("Asp-Net_Core_Final_Project")
    .PersistKeysToFileSystem(keysDirectory);

#endregion



var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<FinalProjectDbContext>(options => options.UseSqlServer(connectionString));




builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserManager>();




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
