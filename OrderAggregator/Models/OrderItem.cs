using System.Text.Json.Serialization;

namespace OrderAggregator.Models;

/// <summary>
/// </summary>
/// <param name="ProductId">Identifikátor</param>
/// <param name="Quantity">Kvantita</param>
public record OrderItem(
    [property: JsonPropertyName("productId")]
    int ProductId,
    [property: JsonPropertyName("quantity")]
    int Quantity);