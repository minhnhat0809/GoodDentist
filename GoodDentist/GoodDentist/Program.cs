
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Impl;
using Services;
using Services.Impl;
using System.Text.Json.Serialization;
using BusinessObject.DTO;
using ClinicService = Services.Impl.ClinicService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IDentistSlotService, DentistSlotService>();

// repo
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IClinicUserRepo, ClinicUserRepo>();
builder.Services.AddScoped<IClinicServiceRepo, ClinicServiceRepo>();
builder.Services.AddScoped<IClinicRepo, ClinicRepo>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDentistSlotRepository, DentistSlotRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});



// database
builder.Services.AddDbContext<GoodDentistDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddStackExchangeRedisCache(redis =>
{
    redis.Configuration = "localhost:6379";
});

//mapper
builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);

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
