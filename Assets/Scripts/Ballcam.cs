using UnityEngine;

public class Ballcam : MonoBehaviour
{
    public GameObject car;
    public GameObject ball;

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
        var carToBall = (ball.transform.position - car.transform.position).normalized;

        var x = carToBall.x * _initialOffset.x;
        var y = _initialOffset.y;
        var z = carToBall.z * _initialOffset.z;

        transform.position = car.transform.position + new Vector3(x, y, z);
        transform.rotation = Quaternion.LookRotation(new Vector3(carToBall.x, _initialRotation.y, carToBall.z));
    }
}
