using OrderAggregator.Services;
using OrderAggregator.Services.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using OrderAggregator.Background;
using OrderAggregator.Background.Options;
using OrderAggregator.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddSingleton<IOrderStore, OrderStore>();

builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<IOrderSender, OrderSender>();

//I added background worker which call SendOrders in intervals
builder.Services.Configure<PeriodicOrderSenderOptions>(
    builder.Configuration.GetSection("PeriodicOrderSender"));
builder.Services.AddHostedService<PeriodicOrderSender>();

//I also have Hangfire Job which do similar job, but it has one issue, when the sender would take longer than interval 
var cronExpression = builder.Configuration.GetValue<string>("HangfireSettings:CronExpression", "*/5 * * * * *");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureOrderApi();

app.UseHangfireDashboard(); // default url is /hangfire

RecurringJob.AddOrUpdate<IOrderSender>("send-orders",
    sender => sender.SendOrdersAsync(),
    cronExpression);

app.Run();