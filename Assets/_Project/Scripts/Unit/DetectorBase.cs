using System;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class DetectorBase : MonoBehaviour
{
    private Unit _unit;

    public event Action<Base> BaseFound;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Base baseResource))
        {
            BaseFound?.Invoke(baseResource);
            _unit.ToggleActive(false);
        }
    }
}