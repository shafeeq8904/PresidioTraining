using BankingAPI.Data;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using BankingAPI.Repositories;
using BankingAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL DbContext
builder.Services.AddDbContext<BankingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register generic repositories
builder.Services.AddScoped<IRepository<int, Account>, AccountRepository>();
builder.Services.AddScoped<IRepository<int, Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<int, TransactionLog>, TransactionLogRepository>();

// Also register concrete repository types (for services that inject them directly)
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<TransactionLogRepository>();

// Register business services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITransactionLogService, TransactionLogService>();

var app = builder.Build();


// Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
