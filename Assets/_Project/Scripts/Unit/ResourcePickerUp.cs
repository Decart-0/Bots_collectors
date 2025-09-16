using UnityEngine;

public class ResourcePickerUp : MonoBehaviour
{
    public void PickUp(Resource resource)
    {
        resource.transform.SetParent(transform);
    }
}