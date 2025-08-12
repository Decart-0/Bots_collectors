using System;
using UnityEngine;

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
        if (collider.TryGetComponent(out Base bas))
        {
            BaseFound?.Invoke(bas);
            _unit.UpdateStatus(false);
        }
    }
}