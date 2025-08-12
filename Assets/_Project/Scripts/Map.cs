using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Map : MonoBehaviour
{
    private Collider _collider;

    public Bounds Bounds => _collider.bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
}