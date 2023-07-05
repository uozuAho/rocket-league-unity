using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public float steerForce;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var forwardInput = Input.GetAxis("Vertical");
        var steerInput = Input.GetAxis("Horizontal");

        _rb.AddForce(Vector3.forward * forwardInput);
        _rb.AddTorque(Vector3.up * steerInput * steerForce);
    }
}
