using System;
using UnityEngine;

public class DetectorBase : MonoBehaviour
{
    public event Action<bool> BaseFound;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Base baseResource))
        {
            BaseFound?.Invoke(false);
        }
    }
}