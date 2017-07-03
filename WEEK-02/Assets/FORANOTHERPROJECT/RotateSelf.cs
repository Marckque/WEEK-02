using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [Header("Rotation")]
    public Vector3 rotation;

    protected void Update()
    {
        transform.Rotate(rotation);
    }
}