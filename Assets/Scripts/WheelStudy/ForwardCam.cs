using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject car;

    private Vector3 _initialOffset;
    private Quaternion _initialRotation;
    private CustomCarControl _carControl;

    private Vector3 camOffset;

    void Start()
    {
        _initialOffset = transform.position;
        _initialRotation = transform.rotation;
        _carControl = GameObject.Find("CustomCar").GetComponent<CustomCarControl>();
    }

    void LateUpdate()
    {
        if (_carControl.IsOnGround)
            camOffset = car.transform.TransformDirection(_initialOffset);

        transform.position = car.transform.position + camOffset;

        if (_carControl.IsOnGround)
            transform.rotation = car.transform.rotation * _initialRotation;
    }
}
