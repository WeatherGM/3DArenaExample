using System;
using System.Collections.Generic;
using System.Linq;

public static class ProbabilityPicker
{
    private static readonly Random _random = new Random();

    public static T PickOne<T>(List<(T item, int weight)> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Список пуст или null.");

        int totalWeight = items.Sum(i => i.weight);

        if (totalWeight <= 0)
            throw new ArgumentException("Сумма весов должна быть больше нуля.");

        int randomValue = _random.Next(totalWeight); 
        int cumulative = 0;

        foreach (var (item, weight) in items)
        {
            cumulative += weight;
            if (randomValue < cumulative) 
                return item;
        }
        return items.Last().item;
    }
}
