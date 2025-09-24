using System;
using UnityEngine;

[RequireComponent(typeof(CounterResources))]
[RequireComponent(typeof(Base))]
public class ResourceCollector : MonoBehaviour
{
    private CounterResources _counterResources;
    private Base _base;

    public event Action<Resource> CollectedResource;

    private void Awake()
    {
        _base = GetComponent<Base>();
        _counterResources = GetComponent<CounterResources>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Unit unit) && unit.IdBase == _base.Id) 
        {
            if (unit.Resource != null)
            {
                _counterResources.AddScore(unit.Resource.Value);
                CollectedResource?.Invoke(unit.Resource);
                unit.ToggleActive(false);
            }
        }
    }  
}