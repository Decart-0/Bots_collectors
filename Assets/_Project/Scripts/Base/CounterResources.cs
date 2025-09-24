using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class CounterResources : MonoBehaviour
{
    [SerializeField] private int _amountUnitSpawn = 3;
    [SerializeField] private int _amountNewBase = 5;
    [SerializeField] private int _resources;

    private Base _base;

    public event Action<int> QuantityChanged;
    public event Action SpawnUnitAllowed;
    public event Action CreatedNewBase;

    private void Awake()
    {
        _base = GetComponent<Base>();
    }

    public void Reset()
    {
        _resources = 0;
        QuantityChanged?.Invoke(_resources);
    }

    public void AddScore(int quantity)
    {
        _resources += quantity;
        QuantityChanged?.Invoke(_resources);
        CheckSpawnPermission();
    }

    public void SubtracteCostSpawnUnit()
    {
        _resources -= _amountUnitSpawn;
        QuantityChanged?.Invoke(_resources);
    }

    public void SubtracteCostSpawnBase()
    {
        _resources -= _amountNewBase;
        QuantityChanged?.Invoke(_resources);
    }

    private void CheckSpawnPermission()
    {
        if (_resources == _amountUnitSpawn && _base.IsSetFlag == false) 
        {
            SpawnUnitAllowed?.Invoke();
        }
        else if(_resources == _amountNewBase && _base.IsSetFlag == true)
        {
            CreatedNewBase?.Invoke();
        }
    }
}