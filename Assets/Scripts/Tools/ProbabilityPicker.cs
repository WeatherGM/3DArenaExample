using System;
using System.Collections.Generic;
using System.Linq;

public static class ProbabilityPicker
{
    private static readonly Random _random = new Random();

    public static T PickOne<T>(List<(T item, int weight)> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("������ ���� ��� null.");

        int totalWeight = items.Sum(i => i.weight);

        if (totalWeight <= 0)
            throw new ArgumentException("����� ����� ������ ���� ������ ����.");

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
