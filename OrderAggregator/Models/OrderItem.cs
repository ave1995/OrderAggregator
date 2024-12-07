using System.Text.Json.Serialization;

namespace OrderAggregator.Models;

public record OrderItem(
    [property: JsonPropertyName("productId")]
    int ProductId,
    [property: JsonPropertyName("quantity")]
    int Quantity);