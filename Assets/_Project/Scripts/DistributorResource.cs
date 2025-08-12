using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NearestResourceScanner))]
[RequireComponent(typeof(Base))]
public class DistributorResource : MonoBehaviour
{
    [SerializeField] private SpawnerResources _spawnerResources;
    
    private Dictionary<Resource, bool> _resources = new Dictionary<Resource, bool>();
    private NearestResourceScanner _nearestResourceScanner;
    private Base _base;

    public int AmountResources => _resources.Count;

    private void Awake()
    {
        _nearestResourceScanner = GetComponent<NearestResourceScanner>();
        _base = GetComponent<Base>();
    }

    private void Update()
    {
        if (_base.GetUnit() != null && HaveActiveResources()) 
        {
            AppointResource();
        }
    }

    public void AddResource(Resource resource)
    {
        _resources.Add(resource, true);
    }

    public void DeleteResource(Resource resource) 
    {
        _resources.Remove(resource);
        Destroy(resource.gameObject);
    }

    private bool HaveActiveResources()
    {
        return _resources.Values.Contains(true);
    }

    private void AppointResource()
    {
        _base.GetUnit().Active(_nearestResourceScanner.SendResource(_resources));
    }
}