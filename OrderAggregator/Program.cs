using OrderAggregator.Services;
using OrderAggregator.Services.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using OrderAggregator.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddSingleton<IOrderStore, OrderStore>();

builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<IOrderSender, OrderSender>();

var cronExpression = builder.Configuration.GetValue<string>("HangfireSettings:CronExpression", "*/5 * * * * *");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureOrderApi();

app.UseHangfireDashboard(); //"/hangfire"

RecurringJob.AddOrUpdate<IOrderSender>("send-orders",
    sender => sender.SendOrdersAsync(),
    cronExpression);

app.Run();