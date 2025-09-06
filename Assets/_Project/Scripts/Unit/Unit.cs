using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [field:SerializeField] public bool IsActive { get; private set; }

    public event Action<Transform> Actived;

    public Base Base { get; private set; }

    private void Awake()
    {
        ToggleActive(false);
    }

    public void Active(Transform resource) 
    {   
        ToggleActive(true);
        Actived?.Invoke(resource);
    }

    public void ToggleActive(bool status)
    {
        IsActive = status;
    }

    public void GetBase(Base baseResource)
    {
        Base = baseResource;
    }
}