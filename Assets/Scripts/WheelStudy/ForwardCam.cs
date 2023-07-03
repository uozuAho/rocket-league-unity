using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject car;

    private Vector3 _initialOffset;
    private Quaternion _initialRotation;


    void Start()
    {
        _initialOffset = transform.position;
        _initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        var offset = car.transform.TransformDirection(_initialOffset);

        transform.position = car.transform.position + offset;
        transform.rotation = car.transform.rotation * _initialRotation;
    }
}
