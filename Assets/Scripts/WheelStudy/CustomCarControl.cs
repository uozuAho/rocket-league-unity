using UnityEngine;

public class CustomCarControl : MonoBehaviour
{
    public float accelPower;
    public float forwardFriction;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        var accelInput = Input.GetAxis("Vertical");

        _rb.AddRelativeForce(transform.forward * accelInput * accelPower * 1000);

        var forwardSpeed = Vector3.Dot(_rb.velocity, transform.forward);
        _rb.AddRelativeForce(-transform.forward * forwardSpeed * forwardFriction * 1000);
    }
}
