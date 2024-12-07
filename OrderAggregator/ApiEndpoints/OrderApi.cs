using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Models;
using OrderAggregator.Services.Interfaces;

namespace OrderAggregator.ApiEndpoints;

public static class OrderApi
{
    public static void ConfigureOrderApi(this WebApplication app)
    {
        app.MapPost("/orders", AddOrders);
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
}