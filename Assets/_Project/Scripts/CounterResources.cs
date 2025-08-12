using System;
using UnityEngine;

public class CounterResources : MonoBehaviour
{
    private int _resources;

    public event Action<int> QuantityChanged;

    public void Reset()
    {
        _resources = 0;
        QuantityChanged?.Invoke(_resources);
    }

    public void AddScore(int quantity)
    {
        _resources += quantity;
        QuantityChanged?.Invoke(_resources);
    }
}