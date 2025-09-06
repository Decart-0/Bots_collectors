using TMPro;
using UnityEngine;

public class ResourcesView : MonoBehaviour
{
    [SerializeField] private CounterResources _counterResources;
    [SerializeField] private TMP_Text _score;

    private void OnEnable()
    {
        _counterResources.QuantityChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _counterResources.QuantityChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int resources)
    {
        _score.text = resources.ToString();
    }
}