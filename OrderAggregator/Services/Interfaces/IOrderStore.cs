using System.Collections.Immutable;
using OrderAggregator.Models;

namespace OrderAggregator.Services.Interfaces;

public interface IOrderStore
{
    Task InsertOrderAsync(OrderItem order);
    
    Task<IImmutableList<OrderItem>> GetCurrentOrdersAsync();
    
    //Napadlo mě přidat tuhle metodu kvůli tomu, že by někdo chtěl dostávat jen aktuální data
    //Například bych jednou za 10 min předal balík změn, vyčistil pamět a začal zase sbírat nové objednávky
    Task<IImmutableList<OrderItem>> ClearOrdersAndGetSnapshotAsync();
}