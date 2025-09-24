using System;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class DetectorResource : MonoBehaviour
{
    private Unit _unit;

    public event Action<Resource> ResourceFound;
    public event Action<Unit> ChangeTarget;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (resource == _unit.Resource)
            {             
                ChangeTarget?.Invoke(_unit);
                ResourceFound?.Invoke(resource);
            }
        }
    }
}