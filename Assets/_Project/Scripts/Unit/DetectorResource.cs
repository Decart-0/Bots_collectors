using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(Unit))]
public class DetectorResource : MonoBehaviour
{
    private UnitMover _unitMotion;
    private Unit _unit;

    public event Action<Resource> ResourceFound;
    public event Action<Unit> ChangeTarget;

    private void Awake()
    {
        _unitMotion = GetComponent<UnitMover>();
        _unit = GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (resource.transform == _unitMotion.TargetPoint)
            {             
                ChangeTarget?.Invoke(_unit);
                ResourceFound?.Invoke(resource);
            }
        }
    }
}