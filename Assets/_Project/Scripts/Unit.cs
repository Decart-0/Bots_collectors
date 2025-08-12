using System;
using UnityEngine;

[RequireComponent(typeof(DetectorBase))]
public class Unit : MonoBehaviour
{
    [field:SerializeField] public bool IsActive { get; private set; }

    public event Action<Transform> Actived;

    private void Awake()
    {
        UpdateStatus(false);
    }

    public void Active(Resource resource) 
    {
        Actived?.Invoke(resource.transform);
        UpdateStatus(true);
    }

    public void UpdateStatus(bool status)
    {
        IsActive = status;
    }
}