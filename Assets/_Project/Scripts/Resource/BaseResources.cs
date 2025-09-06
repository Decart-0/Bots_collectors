using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResources : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Resource> _resources = new List<Resource>();
    [SerializeField] private List<Resource> _resourcesBusy = new List<Resource>();

    public event Action ChangedListResources;

    public float NumberResources => _resources.Count + _resourcesBusy.Count;

    private void OnEnable()
    {
        _scanner.WorkedScanner += UpdateResources;
    }

    private void OnDisable()
    {
        _scanner.WorkedScanner -= UpdateResources;
    }

    public List<Resource> GetResources()
    {
        return _resources;
    }

    public void BorrowResource(Resource resource)
    {
        if (resource == null) return;

        _resources.Remove(resource);
        _resourcesBusy.Add(resource);
    }

    public void DeleteResource(Resource resource)
    {
        _resourcesBusy.Remove(resource);
        ChangedListResources?.Invoke();
    }

    private void UpdateResources(List<Resource> newResources)
    {
        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            Resource currentResource = _resources[i];

            if (newResources.Contains(currentResource) == false)
            {
                if (_resourcesBusy.Contains(currentResource) == false)
                {
                    _resources.RemoveAt(i);
                }
            }
        }

        foreach (Resource component in newResources)
        {
            if (component.TryGetComponent(out Resource resource))
            {
                if (_resources.Contains(resource) == false && _resourcesBusy.Contains(resource) == false)
                {
                    _resources.Add(resource);
                }
            }
        }
    }
}
