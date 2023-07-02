using UnityEngine;

public class CustomCarControl : MonoBehaviour
{
    public float accelPower;
    public float forwardFriction;
    public float sidewaysFriction;
    public float jumpForce;
    public float turnSpeed;

    private Rigidbody _rb;
    private float turnInput;
    private bool jumpPressed;
    private float accelInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        accelInput = Input.GetAxis("Vertical");

        // jump - keydown doesn't register well in FixedUpdate
        if (jumpPressed)
            _rb.AddForce(transform.up * jumpForce * 1000, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(_rb.velocity, transform.forward);

        // steering
        transform.Rotate(transform.up, turnInput * forwardSpeed * turnSpeed * Time.deltaTime);

        // acceleration
        _rb.AddForce(transform.forward * accelInput * accelPower * 1000);

        // forward deceleration due to friction
        _rb.AddForce(-transform.forward * forwardSpeed * forwardFriction * 1000);

        // sideways deceleration due to friction
        var sidewaysSpeed = Vector3.Dot(_rb.velocity, transform.right);
        _rb.AddForce(-transform.right * sidewaysSpeed * sidewaysFriction * 1000);
    }
}
