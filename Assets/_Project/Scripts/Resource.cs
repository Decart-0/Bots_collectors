using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Resource : MonoBehaviour
{
    [SerializeField] private Color _color = new Color(0.090f, 0.262f, 0.830f);
  
    private Renderer _renderer;

    [field:SerializeField] public int Value { get; private set; } = 1;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = _color;
    }
}