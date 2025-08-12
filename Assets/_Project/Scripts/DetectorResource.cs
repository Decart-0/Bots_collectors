using System;
using UnityEngine;

[RequireComponent(typeof(UnitMotion))]
public class DetectorResource : MonoBehaviour
{
    [SerializeField] private Base _base;
    
    private UnitMotion _unitMotion;

    public event Action<Resource> ResourceFound;

    private void Awake()
    {
        _unitMotion = GetComponent<UnitMotion>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (resource.transform == _unitMotion.TargetPoint)
            {
                ResourceFound?.Invoke(resource);
                _unitMotion.SetTarget(_base.transform);
            }
        }
    }
}