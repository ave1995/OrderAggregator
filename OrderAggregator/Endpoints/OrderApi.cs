using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.Endpoints;

public static class OrderApi
{
    public static void ConfigureOrderApi(this WebApplication app)
    {
        app.MapPost("/orders", AddOrders);
        app.MapPost("/order", AddOrder);
    }

    private static async Task<IResult> AddOrders([FromBody]IEnumerable<OrderItem> orders, IOrderManager orderManager)
    {
        try
        {
            await orderManager.AddOrdersAsync(orders);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> AddOrder([FromBody] OrderItem order, IOrderManager orderManager)
    {
        try
        {
            await orderManager.AddOrdersAsync([order]);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}