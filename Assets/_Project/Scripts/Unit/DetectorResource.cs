using System;
using UnityEngine;

[RequireComponent(typeof(UnitMotion))]
[RequireComponent(typeof(Unit))]
public class DetectorResource : MonoBehaviour
{
    private UnitMotion _unitMotion;
    private Unit _unit;

    public event Action<Resource> ResourceFound;

    private void Awake()
    {
        _unitMotion = GetComponent<UnitMotion>();
        _unit = GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (resource.transform == _unitMotion.TargetPoint)
            {
                ResourceFound?.Invoke(resource);
                _unitMotion.SetTarget(_unit.Base.transform);
            }
        }
    }
}