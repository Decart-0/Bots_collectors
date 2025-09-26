using System;
using UnityEngine;

public class Flag : MonoBehaviour 
{
    public event Action<Flag> ReturnedToPool;

    public void Return()
    {
        ReturnedToPool?.Invoke(this);
    }
}