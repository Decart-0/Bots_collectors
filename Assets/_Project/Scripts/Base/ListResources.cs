using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(ResourceCollector))]
public class ListResources : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources = new List<Resource>();
    
    private BaseResources _baseResources;
    private SpawnerResources _spawnerResources;
    private Scanner _scanner;
    private ResourceCollector _resourceCollector;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _resourceCollector = GetComponent<ResourceCollector>();

        _baseResources = FindAnyObjectByType<BaseResources>();
        _spawnerResources = FindAnyObjectByType<SpawnerResources>();
    }

    private void OnEnable()
    {
        _scanner.WorkedScanner += UpdateResourcesBase;
        _baseResources.SyncAllResources += UpdateResourcesBase;
        _resourceCollector.CollectedResource += DeleteResource;
    }

    private void OnDisable()
    {
        _scanner.WorkedScanner -= UpdateResourcesBase;
        _baseResources.SyncAllResources -= UpdateResourcesBase;
        _resourceCollector.CollectedResource -= DeleteResource;
    }

    public IReadOnlyList<Resource> GetResources()
    {
        return _resources.AsReadOnly();
    }

    public void BorrowResource(Resource resource)
    {
        if (resource == null) return;

        _resources.Remove(resource);
        _baseResources.BorrowResource(resource);
    }

    private void DeleteResource(Resource resource)
    {
        _baseResources.DeleteResource(resource);
        _spawnerResources.ReleaseResource(resource);
    }

    private void UpdateResourcesBase()
    {
        UpdateResourcesBase(_resources);
    }

    private void UpdateResourcesBase(List<Resource> newResources)
    {
        _resources = newResources.Where(resource => _baseResources.GetBusyResources().Contains(resource) == false).ToList();
    }
}