using UnityEngine;

[RequireComponent(typeof(DetectorResource))]
public class ResourcePickerUp : MonoBehaviour
{
    private DetectorResource _detectorResource;

    private void Awake()
    {
        _detectorResource = GetComponent<DetectorResource>();
    }

    private void OnEnable()
    {
        _detectorResource.ResourceFound += PickUp;
    }

    private void OnDestroy()
    {
        _detectorResource.ResourceFound -= PickUp;
    }

    private void PickUp(Resource resource)
    {
        resource.transform.SetParent(transform);
    }
}