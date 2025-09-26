using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResources : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources = new List<Resource>();
    [SerializeField] private List<Resource> _resourcesBusy = new List<Resource>();

    public float NumberResources => _resources.Count + _resourcesBusy.Count;

    public event Action SyncAllResources;
    public event Action<Resource> CollectedResources;

    public IReadOnlyList<Resource> GetBusyResources()
    {
        return _resourcesBusy.AsReadOnly();
    }

    public void AddResource(Resource resource)
    {
        _resources.Add(resource);
    }

    public void BorrowResource(Resource resource)
    {
        _resources.Remove(resource);
        _resourcesBusy.Add(resource);
        SyncAllResources?.Invoke();
    }

    public void DeleteResource(Resource resource)
    {
        _resourcesBusy.Remove(resource);
        SyncAllResources?.Invoke();
        CollectedResources?.Invoke(resource);
    }
}